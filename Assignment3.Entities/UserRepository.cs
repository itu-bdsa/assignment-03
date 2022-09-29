namespace Assignment3.Entities;
using Assignment3.Core;
public class UserRepository : IUserRepository
{
    
    private readonly KanbanContext _context;

    public UserRepository(KanbanContext context)
    {
        _context = context;
    }
    public (Response Response, int UserId) Create(UserCreateDTO user) {
        return (Response.BadRequest, 0);
    }

    public IReadOnlyCollection<UserDTO> ReadAll() {
        return null;
    }

    public UserDTO Read(int userId) {
        return null;
    }

    public Response Update(UserUpdateDTO user) {
        return Response.BadRequest;
    }

    public Response Delete(int userId, bool force = false) {
        return Response.BadRequest;
    } 
}
