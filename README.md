# QueryBuilder
Helps to create url query strings

## Usage
### Anonymous models
``` csharp
var builder = new ArraySpecificQueryBuilder();
var query = builder.BuildQuery(new { id = 321, name = "Roman" });
```

**Result:** ``` ?id=321&name=Roman ``` 


### Simple models
``` csharp
internal class SimpleTestModel
{
    public int Id { get; set; }

    public string Name { get; set; }
}
var query = builder.BuildQuery(new SimpleTestModel() { Id = 321, Name = "Roman" });
```
**Result:** `?id=321&name=Roman ` 


### Complex models
You can use complex models with specifying parts such as "method" of request, including parts of this method and string query parts using attributes
``` csharp
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

var query = builder.BuildQuery(new ComplexTestModel() { User = "Roman", 
          Id = "qwerty", 
          Age = 321, 
          ProperyInside = "somevalue" });
```
**Result:** ``` "/api/somevalue/qwerty/others?user=Roman&Age=321" ```



##TODO
Need some investigiations for using Expressions Trees insead of Reflection because of performance issues 
