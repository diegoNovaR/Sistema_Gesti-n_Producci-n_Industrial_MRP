CREATE OR REPLACE FUNCTION trg_crear_inventario_materia()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO Inventario (
        Id_materia_prima,
        stock,
        stock_minimo,
        ubicacion_seccion,
        ubicacion_stand,
        tipo
    )
    VALUES (
        NEW.id,
        0,  -- Stock inicial siempre 0
        3,
        UPPER(SUBSTRING(NEW.Nombre FROM 1 FOR 3)) || '-SEC',
        UPPER(SUBSTRING(NEW.Nombre FROM 1 FOR 3)) || '-STD',
        'Materia Prima'
    );

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER crear_inventario_materia
AFTER INSERT ON MateriaPrima
FOR EACH ROW
EXECUTE FUNCTION trg_crear_inventario_materia();
