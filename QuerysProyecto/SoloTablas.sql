-----SOLO TABLAS

CREATE TABLE MateriaPrima(
	id SERIAL PRIMARY KEY,
	Nombre VARCHAR(30) NOT NULL,
	Descripcion VARCHAR(120) NOT NULL,
	Marca VARCHAR(40) NOT NULL,
	Tipo VARCHAR(20) CHECK (Tipo IN ('procesador', 'ram', 'almacenamiento', 'placa madre', 'pantalla', 'tarjeta video', 'chasis'))
);


CREATE TABLE Proveedor (
    id SERIAL PRIMARY KEY,
    NombreEntidad VARCHAR(30) NOT NULL,
    Direccion VARCHAR(30),
    Identificacion VARCHAR(30) NOT NULL UNIQUE
);

CREATE TABLE OrdenCompra (
    id SERIAL PRIMARY KEY,
    IdMateriaPrima INT NOT NULL REFERENCES MateriaPrima(id) ON DELETE RESTRICT,
    IdProveedor INT NOT NULL REFERENCES Proveedor(id) ON DELETE RESTRICT,
    CantidadUnidades INT NOT NULL CHECK (CantidadUnidades > 0),
    PrecioUnitario DECIMAL(10,2) NOT NULL CHECK (PrecioUnitario > 0),
    PrecioTotal DECIMAL(10,2) GENERATED ALWAYS AS (CantidadUnidades * PrecioUnitario) STORED,
    FechaCompra TIMESTAMP NOT NULL DEFAULT NOW()
);


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










