using LibraryShared.Models;

namespace LibraryBackend.Controllers
{
    public class UserBookController : GenericController<UserBook>
    {
        public UserBookController() : base(new LibraryEntities())
        {
        }
    }
}