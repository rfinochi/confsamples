using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

class DynamicElement : DynamicObject
{
    private Dictionary<string, string> _dictionary;

    private string Name
    {
        get;
        set;
    }

    public DynamicElement( XElement element )
    {
        Name = element.Name.LocalName;
        _dictionary = element.Elements( ).ToDictionary( x => x.Name.LocalName, x => x.Value );
    }

    public override bool TryGetMember( GetMemberBinder binder, out object result )
    {
        if ( _dictionary.ContainsKey( binder.Name ) )
        {
            result = _dictionary[ binder.Name ];

            return true;
        }
        else
        {
            result = null;

            return false;
        }
    }

    public override bool TrySetMember( SetMemberBinder binder, object value )
    {
        _dictionary[ binder.Name ] = value.ToString( );

        return true;
    }

    public XElement ToXml( )
    {
        return new XElement( Name,
            from elem in _dictionary
            select new XElement( elem.Key, elem.Value )
            );
    }
}