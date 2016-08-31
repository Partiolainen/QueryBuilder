using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Attributes;
using QueryBuilder.Implementations;

namespace QueryBuilder.UnitTests
{
    [TestClass]
    public class ArraySpecificQueryBuilderUnitTest
    {
        #region Test Models
        [QueryRequest("/api")]
        internal class ArrayTestModel
        {
            [QueryStringPart("user")]
            public string[] Users { get; set; }
        }

        internal class SimpleTestModelWithIgnore
        {
            public int Id { get; set; }

            [IgnoreQueryPart]
            public string Name { get; set; }
        }

        internal class SimpleTestModel
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        [QueryRequest("/api/complex/{id}")]
        internal class ModelWithQueryParts
        {
            [QueryPart("id")]
            public string SomeProperty { get; set; }
        }

        [QueryRequest("/api/{inside}/{id}/others")]
        internal class ComplexTestModel
        {
            [QueryPart("id")]
            public string Id { get; set; }

            [QueryPart("inside")]
            public string ProperyInside { get; set; }

            [QueryStringPart("user")]
            public string User { get; set; }

            public int Age { get; set; }
        }

        [QueryRequest("/api/{inside}/{id}/others")]
        internal class VeryComplexTestModel
        {
            [QueryPart("id")]
            [QueryStringPart("id")]
            public string Id { get; set; }

            [QueryPart("inside")]
            public string ProperyInside { get; set; }

            [QueryStringPart("user")]
            public string User { get; set; }

            public int Age { get; set; }

            [QueryPart("model")]
            public ComplexTestModel Model { get; set; }

            [IgnoreQueryPart]
            public int AgeOfTime { get; set; }
        }
        #endregion

        [TestMethod]
        public void BuildQueryParametres_WithSimpleValidModel_ReturnsValidQuery()
        {
            var builder = new ArraySpecificQueryBuilder();
            var expected = "?Id=321&Name=Roman";

            var query = builder.BuildQuery(new SimpleTestModel() { Id = 321, Name = "Roman" });

            Assert.AreEqual(expected, query);
        }

        [TestMethod]
        public void BuildQueryParametres_IgnoreParametresWithIgnoreAttribute_ReturnsValidQuery()
        {
            var builder = new ArraySpecificQueryBuilder();
            var expected = "?Id=321";

            var query = builder.BuildQuery(new SimpleTestModelWithIgnore() { Id = 321, Name = "Roman" });

            Assert.AreEqual(expected, query);
        }

        [TestMethod]
        public void BuildQueryParametres_AnonymType_ReturnsValidQuery()
        {
            var builder = new ArraySpecificQueryBuilder();
            var expected = "?Id=321&Name=Roman";

            var query = builder.BuildQuery(new { Id = 321, Name = "Roman" });

            Assert.AreEqual(expected, query);
        }

        [TestMethod]
        public void BuildQueryParametres_ModelWithParts_ReturnsValidQuery()
        {
            var builder = new ArraySpecificQueryBuilder();
            var expected = "/api/complex/Roman";

            var query = builder.BuildQuery(new ModelWithQueryParts() { SomeProperty = "Roman" });

            Assert.AreEqual(expected, query);
        }

        [TestMethod]
        public void BuildQueryParametres_WithComplexValidModel_ReturnsValidQuery()
        {
            var builder = new ArraySpecificQueryBuilder();
            var expected = "/api/somevalue/qwerty/others?user=Roman&Age=321";

            var query = builder.BuildQuery(new ComplexTestModel() { User = "Roman", Id = "qwerty", Age = 321, ProperyInside = "somevalue" });

            Assert.AreEqual(expected, query);
        }

        [TestMethod]
        public void BuildQueryParametres_WithArrayValidModel_ReturnsValidQuery()
        {
            var builder = new ArraySpecificQueryBuilder();
            var expected = "/api?user=Roman&user=Richard&user=Some%20One%20Else";

            var query = builder.BuildQuery(new ArrayTestModel() { Users = new string[] {"Roman", "Richard", "Some One Else" } });

            Assert.AreEqual(expected, query);
        }
    }
}
