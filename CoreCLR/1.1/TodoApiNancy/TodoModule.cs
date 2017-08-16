using Nancy;

using TodoApi.Models;

namespace TodoApi
{
    public class TodoModule : NancyModule
    {
        public TodoModule(ITodoRepository repository)
        {
            Get("/", _ => "TodoApi");
            Get("/api/todo/", _=> repository.AllItems );
            Get("/api/todo/{id}", args => repository.GetById(args.id));
            Delete("/api/todo/{id}", args => repository.Delete(args.id));
        }
    }
}
