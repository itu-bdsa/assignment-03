namespace Assignment3.Entities;

public class UserRepository
{
    private readonly IKanbanContext context;

    public UserRepository(IKanbanContext context)
    {
        this.context = context;
    }

    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        var entry = new User(user.Name, user.Email);

        var e = context.Users
            .Where(usr => usr.Email == entry.Email)
            .FirstOrDefault();

        if(e != null)
        {
            return (Response.Conflict, e.Id);
        }
        else 
        {
            context.Users.Add(entry);
            context.SaveChanges();
            return (Response.Created, entry.Id);
        }
    }

    public Response Delete(int userId, bool force = false)
    {
        var e = context.Users
            .Where(usr => usr.Id == userId)
            .Include(usr => usr.Tasks)
            .FirstOrDefault();

        if (e != null)
        {

            if ((e.Tasks.Count) !=0 )
            {
                if (force) //assigned and forced
                {
                    context.Users.Remove(e);
                    context.SaveChanges();
                    return Response.Deleted;
                }
                else //assigned no force
                {
                    return Response.Conflict;
                }
            }
            else //not assigned
            {
                context.Users.Remove(e);
                context.SaveChanges();
                return Response.Deleted;
            }
        }
        else
        {
            return Response.NotFound;
        }
    }
}
