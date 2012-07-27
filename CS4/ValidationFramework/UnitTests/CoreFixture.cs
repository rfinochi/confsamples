using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ValidationFramework.Dynamic;

namespace ValidationFramework.UnitTests
{
	[TestClass]
	public class CoreFixture
	{
		#region Tests

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		public void StandarRulesIsPersistable( )
		{
			var businessEntity = new BusinessEntity( );

			businessEntity.PersistenceRules.Add( new StandardRule<BusinessEntity>( e => e.Value > 5, "Value must be greater than 5.", "Value" ) );

			businessEntity.Value = 1;
			businessEntity.Value2 = 1;

			Assert.IsFalse( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		public void StandarRulesIsValid( )
		{
			var businessEntity = new BusinessEntity( );

			businessEntity.PersistenceRules.Add( new StandardRule<BusinessEntity>( e => e.Value > 5, "Value must be greater than 5.", "Value" ) );
			businessEntity.BusinessRules.Add( new StandardRule<BusinessEntity>( e => e.Value2 > 5, "Value2 must be greater than 5.", "Value2" ) );

			businessEntity.Value = 6;
			businessEntity.Value2 = 1;

			Assert.IsTrue( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		public void CSharpRulesIsPersistable( )
		{
			IRulesFactory<BusinessEntity> factory = new CSharpRulesFactory<BusinessEntity>( );
			factory.AddRule( RuleType.Persistence, "e.Value > 5", "Value must be greater than 5.", "Value" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( factory.FindRules( RuleType.Persistence ) );

			businessEntity.Value = 1;
			businessEntity.Value2 = 1;

			Assert.IsFalse( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		public void CSharpRuleIsValid( )
		{
			IRulesFactory<BusinessEntity> factory = new CSharpRulesFactory<BusinessEntity>( );
			factory.AddRule( RuleType.Persistence, "e.Value > 5", "Value must be greater than 5.", "Value" );
			factory.AddRule( RuleType.Business, "e.Value2 > 5", "Value2 must be greater than 5.", "Value2" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( factory.FindRules( RuleType.Persistence ) );
			businessEntity.BusinessRules.Add( factory.FindRules( RuleType.Business ) );

			businessEntity.Value = 6;
			businessEntity.Value2 = 1;

			Assert.IsTrue( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		public void PythonRulesIsPersistable( )
		{
			IRulesFactory<BusinessEntity> factory = new PythonRulesFactory<BusinessEntity>( );
			factory.AddRule( 
				RuleType.Persistence, 
				"def validate(e): return e.Value > 5", 
				"Value must be greater than 5.", 
				"Value" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( factory.FindRules( RuleType.Persistence ) );

			businessEntity.Value = 1;
			businessEntity.Value2 = 1;

			Assert.IsFalse( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		public void PythonRulesIsValid( )
		{
			IRulesFactory<BusinessEntity> factory = new PythonRulesFactory<BusinessEntity>( );

			factory.AddRule( 
				RuleType.Persistence, 
				"def validate(e): return e.Value > 5", 
				"Value must be greater than 5.",
				"Value" );
			factory.AddRule( 
				RuleType.Business, 
				"def validate(e): return e.Value2 > 5", 
				"Value2 must be greater than 5.", 
				"Value2" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( factory.FindRules( RuleType.Persistence ) );
			businessEntity.BusinessRules.Add( factory.FindRules( RuleType.Business ) );

			businessEntity.Value = 6;
			businessEntity.Value2 = 1;

			Assert.IsTrue( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		[ExpectedException( typeof( ApplicationException ) )]
		[DeploymentItem( "pythonRules.xml" )]
		[DeploymentItem( "Value3.py" )]
		[DeploymentItem( "Value2.py" )]
		[DeploymentItem( "Value.py" )]
		public void FindAllTypes( )
		{
			IRulesLoader<BusinessEntity, PythonRulesFactory<BusinessEntity>> loader = new XmlRulesLoader<BusinessEntity, PythonRulesFactory<BusinessEntity>>( );

			loader.AddRules( "pythonRules.xml" );

			loader.RulesFactory.FindRules( RuleType.All );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		[DeploymentItem( "pythonRules.xml" )]
		[DeploymentItem( "Value3.py" )]
		[DeploymentItem( "Value2.py" )]
		[DeploymentItem( "Value.py" )]
		public void PythonRulesXmlFileIsValid( )
		{
			IRulesLoader<BusinessEntity, PythonRulesFactory<BusinessEntity>> loader = new XmlRulesLoader<BusinessEntity, PythonRulesFactory<BusinessEntity>>( );
			loader.AddRules( "pythonRules.xml" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( loader.RulesFactory.FindRules( RuleType.Persistence ) );

			businessEntity.Value = 1;
			businessEntity.Value2 = 1;

			Assert.IsFalse( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		[DeploymentItem( "pythonRules.xml" )]
		[DeploymentItem( "Value3.py" )]
		[DeploymentItem( "Value2.py" )]
		[DeploymentItem( "Value.py" )]
		public void PythonRulesXmlFileIsPersistable( )
		{
			IRulesLoader<BusinessEntity, PythonRulesFactory<BusinessEntity>> loader = new XmlRulesLoader<BusinessEntity, PythonRulesFactory<BusinessEntity>>( );
			loader.AddRules( "pythonRules.xml" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( loader.RulesFactory.FindRules( RuleType.Persistence ) );
			businessEntity.BusinessRules.Add( loader.RulesFactory.FindRules( RuleType.Business ) );

			businessEntity.Value = 6;
			businessEntity.Value2 = 1;

			Assert.IsTrue( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		[DeploymentItem( "pythonRules.xml" )]
		[DeploymentItem( "Value3.py" )]
		[DeploymentItem( "Value2.py" )]
		[DeploymentItem( "Value.py" )]
		public void PythonRulesXmlFileIsPersistableAndIsValid( )
		{
			IRulesLoader<BusinessEntity, PythonRulesFactory<BusinessEntity>> loader = new XmlRulesLoader<BusinessEntity, PythonRulesFactory<BusinessEntity>>( );
			loader.AddRules( "pythonRules.xml" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( loader.RulesFactory.FindRules( RuleType.Persistence ) );
			businessEntity.BusinessRules.Add( loader.RulesFactory.FindRules( RuleType.Business ) );

			businessEntity.Value = 6;
			businessEntity.Value2 = 6;
			businessEntity.Value3 = -2;

			Assert.IsFalse( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		[DeploymentItem( "csharpRules.xml" )]
		[DeploymentItem( "Value3.csharp" )]
		[DeploymentItem( "Value2.csharp" )]
		[DeploymentItem( "Value.csharp" )]
		[DeploymentItem( "Complex.csharp" )]
		public void CSharpRulesXmlFileIsValid( )
		{
			IRulesLoader<BusinessEntity, CSharpRulesFactory<BusinessEntity>> loader = new XmlRulesLoader<BusinessEntity, CSharpRulesFactory<BusinessEntity>>( );
			loader.AddRules( "csharpRules.xml" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( loader.RulesFactory.FindRules( RuleType.Persistence ) );

			businessEntity.Value = 1;
			businessEntity.Value2 = 1;

			Assert.IsFalse( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		[DeploymentItem( "csharpRules.xml" )]
		[DeploymentItem( "Value3.csharp" )]
		[DeploymentItem( "Value2.csharp" )]
		[DeploymentItem( "Value.csharp" )]
		[DeploymentItem( "Complex.csharp" )]
		public void CSharpRulesXmlFileIsPersistable( )
		{
			IRulesLoader<BusinessEntity, CSharpRulesFactory<BusinessEntity>> loader = new XmlRulesLoader<BusinessEntity, CSharpRulesFactory<BusinessEntity>>( );
			loader.AddRules( "csharpRules.xml" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( loader.RulesFactory.FindRules( RuleType.Persistence ) );
			businessEntity.BusinessRules.Add( loader.RulesFactory.FindRules( RuleType.Business ) );

			businessEntity.Value = 6;
			businessEntity.Value2 = 1;
			businessEntity.Value4 = 7;
			businessEntity.Value5 = 7;

			Assert.IsTrue( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner( "Rodolfo Finochietti" )]
		[DeploymentItem( "csharpRules.xml" )]
		[DeploymentItem( "Value3.csharp" )]
		[DeploymentItem( "Value2.csharp" )]
		[DeploymentItem( "Value.csharp" )]
		[DeploymentItem( "Complex.csharp" )]
		public void CSharpRulesXmlFileIsPersistableAndIsValid( )
		{
			IRulesLoader<BusinessEntity, CSharpRulesFactory<BusinessEntity>> loader = new XmlRulesLoader<BusinessEntity, CSharpRulesFactory<BusinessEntity>>( );
			loader.AddRules( "csharpRules.xml" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( loader.RulesFactory.FindRules( RuleType.Persistence ) );
			businessEntity.BusinessRules.Add( loader.RulesFactory.FindRules( RuleType.Business ) );

			businessEntity.Value = 6;
			businessEntity.Value2 = 6;
			businessEntity.Value3 = -2;
			businessEntity.Value4 = 7;
			businessEntity.Value5 = 7;

			Assert.IsFalse( businessEntity.IsPersistable( ) );
			Assert.IsFalse( businessEntity.IsValid( ) );
		}

		[TestMethod]
		[Owner("Rodolfo Finochietti")]
		[DeploymentItem( "csharpRules.xml" )]
		[DeploymentItem( "Value3.csharp" )]
		[DeploymentItem( "Value2.csharp" )]
		[DeploymentItem( "Value.csharp" )]
		[DeploymentItem( "Complex.csharp" )]
		public void CSharpRulesXmlFileComplexIsPersistable( )
		{
			IRulesLoader<BusinessEntity, CSharpRulesFactory<BusinessEntity>> loader = new XmlRulesLoader<BusinessEntity, CSharpRulesFactory<BusinessEntity>>( );
			loader.AddRules( "csharpRules.xml" );

			var businessEntity = new BusinessEntity( );
			businessEntity.PersistenceRules.Add( loader.RulesFactory.FindRules( RuleType.Persistence ) );
			businessEntity.BusinessRules.Add( loader.RulesFactory.FindRules( RuleType.Business ) );

			businessEntity.Value = 6;
			businessEntity.Value2 = 6;
			businessEntity.Value3 = 0;
			businessEntity.Value4 = 6;
			businessEntity.Value5 = 6;

			Assert.IsFalse( businessEntity.IsPersistable( ) );

			List<BrokenRuleData> brokenRules = new List<BrokenRuleData>( );
			brokenRules.AddRange( businessEntity.GetAllBrokenRules( ) );

			Assert.AreEqual<int>( 1, brokenRules.Count );

			List<string> brokenRuleProperties = new List<string>( );
			brokenRuleProperties.AddRange( brokenRules[ 0 ].Properties );

			Assert.AreEqual<int>( 2, brokenRuleProperties.Count );

			Assert.AreEqual<string>( "Value4", brokenRuleProperties[ 0 ] );
			Assert.AreEqual<string>( "Value5", brokenRuleProperties[ 1 ] );
		}

		#endregion
	}
}