using Microsoft.AspNetCore.Mvc;

namespace Woodlands_Prototype_Insy7315.Controllers
{
    public class TestimonialsController : Controller
    {
        public IActionResult Index() => View(new List<Woodlands_Prototype_Insy7315.Models.Testimonial>());
    }
}
