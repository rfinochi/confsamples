using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ValidationFramework.Dynamic
{
	public class XmlRulesLoader<TEntity, TRulesFactory> : IRulesLoader<TEntity, TRulesFactory>
		where TRulesFactory : class, IRulesFactory<TEntity>, new( )
	{
		#region IRulesLoader Members

		TRulesFactory _rulesFactory = new TRulesFactory( );

		public TRulesFactory RulesFactory
		{
			get
			{
				return _rulesFactory;
			}
		}

		public void AddRules( string fileOrServerOrConnection )
		{
			XmlDocument doc = new XmlDocument( );
			doc.Load( fileOrServerOrConnection );

			XmlNode root;

			if ( doc.ChildNodes.Count > 1 && doc.FirstChild.NodeType != XmlNodeType.Element )
				root = doc.ChildNodes[ 1 ];
			else
				root = doc.FirstChild;

			foreach ( XmlNode node in root.ChildNodes )
			{
				if ( node.NodeType == XmlNodeType.Element && node.Name != "rules" )
				{
					RuleType type;
					string name = String.Empty;
					string code = null;
					string errorMessage = null;
					List<string> properties = new List<string>( );

					if ( node.Attributes[ "type" ] == null )
						throw new ApplicationException( "Attribute 'type' not found" );
					else
						type = (RuleType)Enum.Parse( typeof( RuleType ), node.Attributes[ "type" ].Value.ToString( ) );

					if ( node.Attributes[ "name" ] != null )
						code = node.Attributes[ "name" ].Value.ToString( );

					if ( node.Attributes[ "code" ] == null )
						throw new ApplicationException( "Attribute 'code' not found" );
					else
						code = File.ReadAllText( node.Attributes[ "code" ].Value.ToString( ) );

					if ( node.Attributes[ "errorMessage" ] == null )
						throw new ApplicationException( "Attribute 'code' not found" );
					else
						errorMessage = node.Attributes[ "errorMessage" ].Value.ToString( );

					foreach ( XmlNode childNode in node.ChildNodes )
					{
						if ( childNode.NodeType == XmlNodeType.Element )
						{
							if ( childNode.Attributes[ "value" ] == null )
								throw new ApplicationException( "Attribute 'property\value' not found" );
							else
								properties.Add( childNode.Attributes[ "value" ].Value.ToString( ) );
						}
					}

					_rulesFactory.AddRule( type, code, errorMessage, properties.ToArray( ) );
				}
			}
		}

		#endregion
	}
}