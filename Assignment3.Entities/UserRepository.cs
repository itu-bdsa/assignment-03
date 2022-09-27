using Assignment3.Core;
namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{
    private readonly KanbanContext context;

    public UserRepository(KanbanContext _context)
    {
        context = _context;
    }
    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        var entity = new User
        {
            name = user.Name,
            email = user.Email
        };

        context.Users.Add(entity);
        context.SaveChanges();

        return (Response.Created, entity.id);
    }

    public Response Delete(int userId, bool force = false)
    {
        var entity = context.Users.Find(userId);

        if (entity == null)
        {
            return Response.NotFound;
        }

        if (entity.tasks.Count > 0 && !force)
        {
            return Response.Conflict;
        }

        context.Users.Remove(entity);
        context.SaveChanges();

        return Response.Deleted;
    }

    public UserDTO Read(int userId)
    {
        var entity = context.Users.Find(userId);

        if (entity == null)
        {
            return null;
        }

        return new UserDTO(entity.id, entity.name, entity.email);
    }

    public IReadOnlyCollection<UserDTO> ReadAll()
    {
        var users = from c in context.Users
                    orderby c.name
                    select new UserDTO(c.id, c.name, c.email);

        return users.ToArray();
    }

    public Response Update(UserUpdateDTO user)
    {
        var entity = context.Users.Find(user.Id);

        if (entity == null)
        {
            return Response.NotFound;
        }

        entity.name = user.Name;
        entity.email = user.Email;

        context.SaveChanges();

        return Response.Updated;
    }
}
