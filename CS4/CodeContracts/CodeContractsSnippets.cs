#region Snippet 0 - Clase Factura

public class Factura
{
	//Propiedades
	private DateTime _fechaEntrega;
	public DateTime FechaEntrega
	{
		get
		{
			return _fechaEntrega;
		}
	}

	private DateTime _fechaFactura;
	public DateTime FechaFactura
	{
		get
		{
			return _fechaFactura;
		}
	}

	int _monto;
	public int Monto
	{
		get
		{
			return _monto;
		}
		set
		{
			_monto = value;
		}
	}

	//Constructor
	public Factura(DateTime fechaEntrega, DateTime fechaFactura, int monto)
	{
		_fechaEntrega = fechaEntrega;
		_fechaFactura = fechaFactura;
		_monto = monto;
	}

	//Metodos publicos
	public void AplicarDescuento(int porcentaje)
	{
		int descuento = (porcentaje * _monto) / 100;
		_monto = _monto - descuento;
	}
}

#endregion


#region Snippet 1 - Invariante de tipo
[ContractInvariantMethod]
void ObjectInvariant()
{
	Contract.Invariant(_fechaEntrega >= _fechaFactura);
	Contract.Invariant(_monto >= 0);
}
#endregion


#region Snippet 2 - Constructor
public Factura(DateTime fechaEntrega, DateTime fechaFactura, int monto)
{
	Contract.Requires(fechaEntrega >= fechaFactura);
	Contract.Requires(monto >= 0);

	_fechaEntrega = fechaEntrega;
	_fechaFactura = fechaFactura;
	_monto = monto;
}
#endregion

#region Snippet 2 - Aplicar descuento
public void AplicarDescuento(int porcentaje)
{
	Contract.Requires(porcentaje >= 0);
	Contract.Requires(porcentaje <= 100);
	Contract.Ensures(Contract.OldValue(_monto) >= _monto);

	int descuento = (porcentaje * _monto) / 100;
	Contract.Assume(descuento <= _monto);

	_monto = _monto - descuento;
}
#endregion

#region Snippet 2 - Propiedades
int _monto;
public int Monto
{
	get
	{
		return _monto;
	}
	set
	{
		Contract.Requires(value >= 0);	
		_monto = value;
	}
}
#endregion 




#region Snippet 3 - Programa solo

Console.Write("Ingrese la fecha de la factura: ");
DateTime fechaFactura = DateTime.Parse(Console.ReadLine());

Console.Write("Ingrese la fecha de entrega: ");
DateTime fechaEntega = DateTime.Parse(Console.ReadLine());

Console.Write("Ingrese el monto: ");
string nro = Console.ReadLine();
int monto = Int32.Parse(nro);

Factura f = new Factura(fechaEntega, fechaFactura, monto);
Console.WriteLine("Producto facturado!");

Console.ReadKey();

#endregion


#region Snippet 4 - Programa con contratos

Console.Write("Ingrese la fecha de la factura: ");
DateTime fechaFactura = DateTime.Parse(Console.ReadLine());

Console.Write("Ingrese la fecha de entrega: ");
DateTime fechaEntega = DateTime.Parse(Console.ReadLine());

Console.Write("Ingrese el monto: ");
string nro = Console.ReadLine();
Contract.Assume(nro != null); // Int32.Parse tiene los Require incluidos!
int monto = Int32.Parse(nro);

if (fechaEntega >= fechaFactura && monto > 0)
{
	Factura f = new Factura(fechaEntega, fechaFactura, monto);
	Console.WriteLine("Producto facturado!");
}
else
{
	Console.WriteLine("Valores incorrectos!");
}

Console.ReadKey();
#endregion

#region Snippet 5 - Contract Failed Handler
Console.WriteLine("No se pudo asumir algo: " + e.Condition);
e.SetHandled();
Console.ReadKey();
#endregion