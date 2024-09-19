using LibraryShared.Models;

namespace LibraryBackend.Controllers
{
    public class UserWorkshopController : GenericController<UserWorkshop>
    {
        public UserWorkshopController() : base(new LibraryEntities())
        {
        }
    }
}