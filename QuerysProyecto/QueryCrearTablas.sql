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
	Direccion VARCHAR(30) NULL,
	Identificacion VARCHAR(30) NOT NULL
)

INSERT INTO Proveedor (NombreEntidad, Direccion, Identificacion)
VALUES ('Entidad Prueba', 'Direccion Prueba urb. Prueba', 'Identifacion prueba123')
------------------------------------------------------------------------------------------------
CREATE TABLE OrdenCompra(
	id SERIAL PRIMARY KEY,
	IdMateriaPrima INT NOT NULL REFERENCES MateriaPrima(id),
	IdProveedor INT NOT NULL REFERENCES Proveedor(id),
	CantidadUnidades INt NOT NULL CHECK (CantidadUnidades > 0),
	PrecioUnitario DECIMAL(10,2) NOT NULL CHECK (PrecioUnitario > 0), 
	PrecioTotal DECIMAL(10,2) GENERATED ALWAYS AS (CantidadUnidades * PrecioUnitario) STORED,
	FechaCompra TIMESTAMP NOT NULL DEFAULT NOW()
)

----------------------------------------------------------------------
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
----------------------------------------------------
