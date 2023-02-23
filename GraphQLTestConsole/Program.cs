using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQL.SystemTextJson;



var schema = new Schema { Query = new MyTypes.StarWarsQuery() };

var json = await schema.ExecuteAsync(_ =>
{
    _.Query = "{ hero { id,name, } }";
});

Console.WriteLine(json);



namespace MyTypes
{
    public class Droid
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType()
        {
            Field(x => x.Id).Description("The Id of the Droid.");
            Field(x => x.Name).Description("The name of the Droid.");
        }
    }

    public class StarWarsQuery : ObjectGraphType
    {
        public StarWarsQuery()
        {
            Field<DroidType>("hero")
                .Resolve(context => new Droid { Id = "1", Name = "R2-D2" });
        }
    }
}