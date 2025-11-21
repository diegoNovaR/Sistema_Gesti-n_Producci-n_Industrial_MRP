CREATE TABLE MateriaPrima(
	id SERIAL PRIMARY KEY,
	Nombre VARCHAR(30) NOT NULL,
	Descripcion VARCHAR(120) NOT NULL,
	Marca VARCHAR(40) NOT NULL,
	Tipo VARCHAR(20) CHECK (Tipo IN ('procesador', 'ram', 'almacenamiento', 'placa madre', 'pantalla', 'tarjeta video', 'chasis'))
);


INSERT INTO MateriaPrima (Nombre, Descripcion, Marca, Tipo)
VALUES ('Procesador Prueba', 'Descripcion Prueba', 'Marca Prueba', 'procesador')

SELECT * FROM MateriaPrima

----------------------------------------------------------------------------------------------
CREATE TABLE Proveedor (
    id SERIAL PRIMARY KEY,
    NombreEntidad VARCHAR(30) NOT NULL,
    Direccion VARCHAR(30),
    Identificacion VARCHAR(30) NOT NULL UNIQUE
);

INSERT INTO Proveedor (NombreEntidad, Direccion, Identificacion)
VALUES ('Entidad Prueba', 'Direccion Prueba urb. Prueba', 'Identifacion prueba123')

SELECT * FROM Proveedor
------------------------------------------------------------------------------------------------
CREATE TABLE OrdenCompra (
    id SERIAL PRIMARY KEY,
    IdMateriaPrima INT NOT NULL REFERENCES MateriaPrima(id) ON DELETE RESTRICT,
    IdProveedor INT NOT NULL REFERENCES Proveedor(id) ON DELETE RESTRICT,
    CantidadUnidades INT NOT NULL CHECK (CantidadUnidades > 0),
    PrecioUnitario DECIMAL(10,2) NOT NULL CHECK (PrecioUnitario > 0),
    PrecioTotal DECIMAL(10,2) GENERATED ALWAYS AS (CantidadUnidades * PrecioUnitario) STORED,
    FechaCompra TIMESTAMP NOT NULL DEFAULT NOW()
);

---------------------------INSERTAR UNA ORDEN DE COMPRA-------------------------------------------
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
BEGIN
    -- Validamos Materia Prima
    IF NOT EXISTS (SELECT 1 FROM MateriaPrima WHERE id = p_idmateriaprima) THEN
        RAISE EXCEPTION 'La MateriaPrima con id % no existe', p_idmateriaprima;
    END IF;

    -- Validamos Proveedor
    IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE id = p_idproveedor) THEN
        RAISE EXCEPTION 'El Proveedor con id % no existe', p_idproveedor;
    END IF;

    -- Insertamos orden de compra
    INSERT INTO OrdenCompra (
        IdMateriaPrima, IdProveedor, CantidadUnidades, PrecioUnitario
    )
    VALUES (
        p_idmateriaprima, p_idproveedor, p_cantidad, p_precio_unitario
    )
    RETURNING id INTO new_id;

    RETURN new_id;
END;
$$ LANGUAGE plpgsql;
----------------------------------------------------
SELECT * FROM OrdenCompra


------------Para devolver Todas las ORDENES de COmMPRA-----------------------------------------
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
----------------------INSERTAR MATERIA PRIMA------------------------------
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
	
-------------------------------------------------------

-- Insertar dato de un nuevo procesador
SELECT sp_insertar_materia_prima(
    'Intel Core i7-14700K', 
    'Procesador Intel Core i7 14ma generación 16 nucleos 16 hilos', 
    'Intel', 
    'procesador'
);

SELECT * FROM MateriaPrima
------------------------------------------------------------
--------Funcion para crear un nuevo Proveedor
CREATE OR REPLACE FUNCTION sp_insertar_proveedor(
	nombreEntidad VARCHAR(30),
	direccion VARCHAR(30),
	identificacion_ VARCHAR(30)
)
RETURNS INT AS
$$
DECLARE
	new_id INT;
BEGIN
	--validamos que la identificacion sea unica
	IF EXISTS(SELECT 1 FROM Proveedor WHERE Identificacion = identificacion_) THEN
		RAISE EXCEPTION 'Ya existe un proveedor con esta identificacion %', identificacion_;
	END IF;

	INSERT INTO Proveedor (NombreEntidad, Direccion, Identificacion)
	VALUES (nombreEntidad, direccion, identificacion_)
	RETURNING id INTO new_id;
	RETURN new_id;
END;
$$ LANGUAGE plpgsql;
 
SELECT sp_insertar_proveedor('REMATAZO', 'Arequipa novacenter', '1040050')

SELECT * FROM Proveedor

------------------------ESTO ES PARA DEVOLVER LISTAS DE MATS Y PROVEEDORES PARA FORMULARI-----------------------------------------
----------------------------------Para Mats---------------------------------------
SELECT id, Nombre
FROM MateriaPrima
ORDER BY id DESC;
----------------------------------Para Proveedores-------------------
SELECT id, NombreEntidad
FROM Proveedor
ORDER BY id DESC; 
-----------------------------------------------------------------------------------
-----------------------------------------------------------------------------------


------------------------------------------------------------------------------
---------------------------Laptop, BOM o Receta, Detalle BOM------------------------------------------------
CREATE TABLE Laptop (
    id SERIAL PRIMARY KEY,
    Nombre VARCHAR(30) NOT NULL UNIQUE,
    Tipo VARCHAR(20) NOT NULL CHECK (Tipo IN ('gamer', 'básica')),
    FechaCreacion TIMESTAMP NOT NULL DEFAULT NOW(),
    PrecioVenta DECIMAL(10,2) NOT NULL CHECK (PrecioVenta >= 0),
    CostoProduccion DECIMAL(10,2) NOT NULL DEFAULT 0 CHECK (CostoProduccion >= 0)
);


CREATE TABLE Receta (
    id SERIAL PRIMARY KEY,
    IdLaptop INT NOT NULL UNIQUE REFERENCES Laptop(id) ON DELETE CASCADE,--una lap tiene 1 receta
    FechaCreacion TIMESTAMP NOT NULL DEFAULT NOW(),
    FechaModificacion TIMESTAMP,
    CostoTotalDeReceta DECIMAL(10,2) NOT NULL DEFAULT 0 CHECK (CostoTotalDeReceta >= 0)
);

CREATE TABLE DetalleReceta (
    id SERIAL PRIMARY KEY,
    IdReceta INT NOT NULL REFERENCES Receta(id) ON DELETE CASCADE,
    IdMateriaPrima INT NOT NULL REFERENCES MateriaPrima(id),
    CantidadMateriaPrima INT NOT NULL CHECK (CantidadMateriaPrima > 0),
    UNIQUE (IdReceta, IdMateriaPrima)
);

CREATE TABLE orden_produccion (
    id SERIAL PRIMARY KEY,
    id_receta INT NOT NULL REFERENCES Receta(id) ON DELETE CASCADE,
    cantidad_producto_fabricado INT NOT NULL CHECK (cantidad_producto_fabricado > 0),
    costo_produccion DECIMAL(10,2) NOT NULL CHECK (costo_produccion >= 0)
);

CREATE TABLE Lote (
    id SERIAL PRIMARY KEY,
    id_orden_produccion INT NOT NULL REFERENCES orden_produccion(id) ON DELETE CASCADE,
    codigo_lote VARCHAR(10) NOT NULL UNIQUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT NOW(),
    cantidad_producto_fabricado INT NOT NULL CHECK (cantidad_producto_fabricado > 0)
);

CREATE TABLE Inventario (
    id SERIAL PRIMARY KEY,
    Id_laptop INT REFERENCES Laptop(id),
    Id_materia_prima INT REFERENCES MateriaPrima(id),
    stock INT NOT NULL CHECK (stock >= 0),
    stock_minimo INT NOT NULL CHECK (stock_minimo >= 0),
    ubicacion_seccion VARCHAR(10) NOT NULL,
    ubicacion_stand VARCHAR(10) NOT NULL,
    tipo VARCHAR(20) NOT NULL CHECK (tipo IN ('Materia Prima', 'Laptop')),

    CHECK (
        (Id_laptop IS NOT NULL AND Id_materia_prima IS NULL AND tipo = 'Laptop') OR
        (Id_laptop IS NULL AND Id_materia_prima IS NOT NULL AND tipo = 'Materia Prima')
    )
);

CREATE TABLE Inventario_movimiento (
    id SERIAL PRIMARY KEY,
    id_inventario INT NOT NULL REFERENCES Inventario(id) ON DELETE CASCADE,
    tipo_movimiento VARCHAR(20) NOT NULL CHECK (tipo_movimiento IN ('entrada', 'produccion salida', 'ajuste')),
    referencia VARCHAR(30),
    fecha_movimiento TIMESTAMP NOT NULL DEFAULT NOW(),
    stock_anterior INT NOT NULL CHECK (stock_anterior >= 0),
    stock_actual INT NOT NULL CHECK (stock_actual >= 0)
);

---------------------------------------------------------------------
--------------------- CREAR LAPTOP CON RECETA Y DetalleReceta ----------------------

------------------------------------------------------------------------
---------------------------------------------------------------------------------------
CREATE OR REPLACE FUNCTION Crear_Laptop_Receta(
    p_nombre VARCHAR,
    p_tipo VARCHAR,
    p_precioVenta DECIMAL(10,2),
    p_detalles TEXT -- <--- ahora es TEXT, ya no JSON
)
RETURNS TABLE(laptop_id INT, receta_id INT, costo_total DECIMAL) AS $$--aqui hacer que no se retorne nada
DECLARE
    v_idLaptop INT;
    v_idReceta INT;
    v_totalCosto DECIMAL(10,2) := 0;
    item JSON;
    v_idMateria INT;
    v_cantidad INT;
    v_precioUnitario DECIMAL(10,2);
BEGIN
    -- Validaciones iniciales
    IF p_tipo NOT IN ('gamer', 'básica') THEN
        RAISE EXCEPTION 'Tipo de laptop inválido: %. Debe ser "gamer" o "básica"', p_tipo;
    END IF;

    -- Insertar laptop
    INSERT INTO Laptop (Nombre, Tipo, PrecioVenta, CostoProduccion)
    VALUES (p_nombre, p_tipo, p_precioVenta, 0)
    RETURNING id INTO v_idLaptop;

    -- Crear receta
    INSERT INTO Receta (IdLaptop, CostoTotalDeReceta)
    VALUES (v_idLaptop, 0)
    RETURNING id INTO v_idReceta;

    -- Procesar detalles (convertimos TEXT → JSON)
    FOR item IN SELECT * FROM json_array_elements(p_detalles::json)
    LOOP
        v_idMateria := (item->>'idMateriaPrima')::INT;
        v_cantidad  := (item->>'cantidad')::INT;

        IF v_cantidad <= 0 THEN
            RAISE EXCEPTION 'Cantidad inválida: %', v_cantidad;
        END IF;

        -- Obtener precio más reciente
        SELECT PrecioUnitario
        INTO v_precioUnitario
        FROM OrdenCompra 
        WHERE IdMateriaPrima = v_idMateria
        ORDER BY FechaCompra DESC 
        LIMIT 1;

        IF NOT FOUND THEN
            RAISE EXCEPTION 'No se encontró precio para materia prima ID: %', v_idMateria;
        END IF;

        -- Insertar detalle
        INSERT INTO DetalleReceta (IdReceta, IdMateriaPrima, CantidadMateriaPrima)
        VALUES (v_idReceta, v_idMateria, v_cantidad);

        v_totalCosto := v_totalCosto + (v_cantidad * v_precioUnitario);
    END LOOP;

    -- Actualizar costos
    UPDATE Receta SET CostoTotalDeReceta = v_totalCosto WHERE id = v_idReceta;
    UPDATE Laptop SET CostoProduccion = v_totalCosto WHERE id = v_idLaptop;

    -- Retornar resultados
    RETURN QUERY SELECT v_idLaptop, v_idReceta, v_totalCosto;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM Laptop;
SELECT * FROM DetalleReceta;
SELECT * FROM Receta;
SELECT * FROM MateriaPrima
SELECT * FROM OrdenCompra ORDER BY FechaCompra

SELECT proname, proargtypes
FROM pg_proc
WHERE proname ILIKE '%laptop%';

