----------------------FUNCIONES------------
-------------------------INSERTAR MATERIA PRIMA---------------------------------------------

CREATE OR REPLACE FUNCTION sp_insertar_materia_prima(
	nombre varchar(30),
	descripcion varchar(120),
	marca varchar(40),
	tipo varchar(20)
)
RETURNS INT AS
$$
DECLARE
	new_id INT;
BEGIN
	IF tipo NOT IN('procesador', 'ram', 'almacenamiento', 'placa madre', 'pantalla', 'tarjeta video', 'chasis') THEN
        RAISE EXCEPTION 'El tipo % no es válido', tipo;
    END IF;
	INSERT INTO MateriaPrima(Nombre, Descripcion, Marca, Tipo)
	VALUES (nombre, descripcion, marca, tipo)
	RETURNING id INTO new_id;

	RETURN new_id;
END;
$$ LANGUAGE plpgsql;

SELECT sp_insertar_materia_prima('Mats 1 prueba', ' prueba' , 'prueba', 'procesador')

-------------------------------------------------------------------
--------------------------INSERTAR PROVEEDOR------------------------------------------------------

SELECT * FROM proveedor
-----------------------------------------------------------------------------------
-------------INSERTAR ORDEN COMPRA CON REGISTRO --------------------------
CREATE OR REPLACE FUNCTION sp_insertar_orden_compra(
    p_idmateriaprima INT,
    p_idproveedor INT,
    p_cantidad INT,
    p_precio_unitario DECIMAL(10,2)
)
RETURNS INT AS
$$
DECLARE
    new_id INT;
    v_inventario_id INT;
    v_stock_anterior INT;
    v_stock_actual INT;
BEGIN
    -- Validamos Materia Prima
    IF NOT EXISTS (SELECT 1 FROM MateriaPrima WHERE id = p_idmateriaprima) THEN
        RAISE EXCEPTION 'La MateriaPrima con id % no existe', p_idmateriaprima;
    END IF;

    -- Validamos Proveedor
    IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE id = p_idproveedor) THEN
        RAISE EXCEPTION 'El Proveedor con id % no existe', p_idproveedor;
    END IF;

    -- Validamos cantidad y precio
    IF p_cantidad <= 0 THEN
        RAISE EXCEPTION 'La cantidad debe ser mayor a 0';
    END IF;

    IF p_precio_unitario <= 0 THEN
        RAISE EXCEPTION 'El precio unitario debe ser mayor a 0';
    END IF;

    -- BLOQUEO: Buscamos y bloqueamos el registro de inventario
    SELECT id, stock INTO v_inventario_id, v_stock_anterior
    FROM Inventario 
    WHERE Id_materia_prima = p_idmateriaprima AND tipo = 'Materia Prima'
    FOR UPDATE;  --BLOQUEO PARA ACID

    IF v_inventario_id IS NULL THEN
        RAISE EXCEPTION 'No existe registro en inventario para la materia prima con id %', p_idmateriaprima;
    END IF;

    -- Insertamos orden de compra
    INSERT INTO OrdenCompra (
        IdMateriaPrima, IdProveedor, CantidadUnidades, PrecioUnitario
    )
    VALUES (
        p_idmateriaprima, p_idproveedor, p_cantidad, p_precio_unitario
    )
    RETURNING id INTO new_id;

    -- Actualizar stock existente en inventario 
    UPDATE Inventario 
    SET stock = stock + p_cantidad
    WHERE id = v_inventario_id
    RETURNING stock INTO v_stock_actual;

    -- Registrar movimiento en inventario_movimiento
    INSERT INTO Inventario_movimiento (
        id_inventario, 
        tipo_movimiento, 
        referencia, 
        stock_anterior, 
        stock_actual
    )
    VALUES (
        v_inventario_id,
        'entrada',
        'OC-' || new_id,
        v_stock_anterior,
        v_stock_actual
    );

    RETURN new_id;

EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error en la transacción: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM OrdenCompra
SELECT * FROM Inventario
SELECT * FROM inventario_movimiento
----------------------------------------------------------

SELECT * FROM materiaprima

DELETE FROM materiaprima;

------------------LISTADO DE ORDENES COMPRA----------------------------------------------

CREATE OR REPLACE FUNCTION fn_Lista_Ordenes_Compra()
RETURNS TABLE (
    id INT,
    materiaprima VARCHAR(30),
    proveedor VARCHAR(30),
    cantidadunidades INT,
    preciounitario DECIMAL(10,2),
    preciototal DECIMAL(10,2),
    fechacompra TIMESTAMP
)
AS
$$
BEGIN
    RETURN QUERY
    SELECT 
        oc.id,
        mp.Nombre AS MateriaPrima,
        pr.NombreEntidad AS Proveedor,
        oc.CantidadUnidades,
        oc.PrecioUnitario,
        oc.PrecioTotal,
        oc.FechaCompra
    FROM OrdenCompra oc
    INNER JOIN MateriaPrima mp ON mp.id = oc.IdMateriaPrima
    INNER JOIN Proveedor pr ON pr.id = oc.IdProveedor
    ORDER BY oc.id DESC;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM fn_Lista_Ordenes_Compra()


----------------------------------------------------------

----------------------------------------------------------


CREATE OR REPLACE FUNCTION sp_crear_laptop_con_receta(
    p_nombre VARCHAR,
    p_tipo VARCHAR,
    p_precio_venta DECIMAL(10,2),
    p_detalles JSON  
)
RETURNS BOOLEAN AS
$$
DECLARE
    v_laptop_id INT;
    v_receta_id INT;
    v_costo_total DECIMAL(10,2) := 0;
    v_mi JSON;
    v_id_materia INT;
    v_cantidad INT;
    v_stock_actual INT;
    v_stock_nuevo INT;
    v_precio_unitario DECIMAL(10,2);
BEGIN
    ------------------------------------------------------------
    -- VALIDACIONES GENERALES
    ------------------------------------------------------------
    IF p_nombre IS NULL OR p_tipo IS NULL THEN
        RAISE EXCEPTION 'Nombre y tipo no pueden ser nulos';
    END IF;

    IF p_tipo NOT IN ('gamer', 'básica') THEN
        RAISE EXCEPTION 'Tipo de laptop inválido';
    END IF;

    ------------------------------------------------------------
    -- 1. INSERTAR LAPTOP
    ------------------------------------------------------------
    INSERT INTO Laptop (Nombre, Tipo, PrecioVenta, CostoProduccion)
    VALUES (p_nombre, p_tipo, p_precio_venta, 0)
    RETURNING id INTO v_laptop_id;

    ------------------------------------------------------------
    -- 2. CREAR RECETA ASOCIADA
    ------------------------------------------------------------
    INSERT INTO Receta (IdLaptop, CostoTotalDeReceta)
    VALUES (v_laptop_id, 0)
    RETURNING id INTO v_receta_id;

    ------------------------------------------------------------
    -- 3. PROCESAR CADA MATERIAL DEL JSON
    ------------------------------------------------------------
    FOR v_mi IN SELECT * FROM json_array_elements(p_detalles)
    LOOP
        v_id_materia := (v_mi->>'id_materia')::INT;
        v_cantidad   := (v_mi->>'cantidad')::INT;

        -- VALIDAR MATERIA PRIMA EXISTE
        IF NOT EXISTS (SELECT 1 FROM MateriaPrima WHERE id = v_id_materia) THEN
            RAISE EXCEPTION 'La materia prima % no existe', v_id_materia;
        END IF;

        -- BLOQUEAR INVENTARIO
        SELECT stock INTO v_stock_actual
        FROM Inventario
        WHERE id_materia_prima = v_id_materia AND tipo = 'Materia Prima'
        FOR UPDATE;

        IF v_stock_actual IS NULL THEN
            RAISE EXCEPTION 'La materia prima % no tiene registro en inventario', v_id_materia;
        END IF;

        -- VALIDAR STOCK SUFICIENTE
        IF v_stock_actual < v_cantidad THEN
            RAISE EXCEPTION 'Stock insuficiente para materia % (requiere %, disponible %)',
                v_id_materia, v_cantidad, v_stock_actual;
        END IF;

        -- OBTENER PRECIO DE ÚLTIMA ORDEN DE COMPRA
        SELECT PrecioUnitario INTO v_precio_unitario
        FROM OrdenCompra
        WHERE IdMateriaPrima = v_id_materia
        ORDER BY FechaCompra DESC
        LIMIT 1;

        IF v_precio_unitario IS NULL THEN
            RAISE EXCEPTION 'La materia prima % no tiene precio registrado en ordenes de compra', v_id_materia;
        END IF;

        -- ACUMULAR COSTO TOTAL
        v_costo_total := v_costo_total + (v_precio_unitario * v_cantidad);

        -- DESCONTAR INVENTARIO
        v_stock_nuevo := v_stock_actual - v_cantidad;
        UPDATE Inventario
        SET stock = v_stock_nuevo
        WHERE id_materia_prima = v_id_materia
        AND tipo = 'Materia Prima';

        -- CREAR DETALLE DE RECETA
        INSERT INTO DetalleReceta (IdReceta, IdMateriaPrima, CantidadMateriaPrima)
        VALUES (v_receta_id, v_id_materia, v_cantidad);
    END LOOP;

    ------------------------------------------------------------
    -- 4. ACTUALIZAR COSTOS
    ------------------------------------------------------------
    UPDATE Receta
    SET CostoTotalDeReceta = v_costo_total,
        FechaModificacion = NOW()
    WHERE id = v_receta_id;

    UPDATE Laptop
    SET CostoProduccion = v_costo_total
    WHERE id = v_laptop_id;

    RETURN TRUE;

EXCEPTION WHEN OTHERS THEN
    RAISE NOTICE 'Error: %', SQLERRM;
    RETURN FALSE;
END;
$$ LANGUAGE plpgsql; 









 

