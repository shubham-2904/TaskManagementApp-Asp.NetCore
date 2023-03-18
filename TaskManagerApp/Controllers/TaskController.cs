using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagerApp.Models;
using TaskManagerApp.Models.Domain;

namespace TaskManagerApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskManagerDbContext context;

        public TaskController()
        {
            this.context = new TaskManagerDbContext();
        }

        /*
            Sinpup and login
         */
        [HttpGet]
        [Route("/signup")]
        public IActionResult Signup()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View("Signup");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult VerifyLogin(LoginRequest userLogin)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var user = context.Users
                .Where(u => u.UserEmail == userLogin.Email && u.UserPassword == userLogin.Password)
                .FirstOrDefault();

                if (user != null)
                {
                    HttpContext.Session.SetString("UserName", user.UserName.ToString());
                    HttpContext.Session.SetString( "UserId", user.Id.ToString());

                    return RedirectToAction("Index");

                }
                else
                {
                    return View("Login");

                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult AddUser(AddUserRequest addUser)
        {            
            var user = context.Users.Where(u => u.UserEmail == addUser.UserEmail).FirstOrDefault();

            if (user != null)
            {
                return Ok("User Already exsits");
            }

            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                UserEmail = addUser.UserEmail,
                UserPassword = addUser.UserPassword,
                UserName = addUser.UserName
            };

            context.Users.Add(newUser);
            context.SaveChanges();

            return Redirect("/login");
        }



        /*
         * Starting List
         */

        // Index page action
        [HttpGet]
        [Authentication]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            Guid UserID = new Guid( userId );

            var list = await context.Lists.Where( l => l.UserId == UserID).ToListAsync();

            //var list = await context.Lists.ToListAsync();


            if (HttpContext.Session.GetString("UserName") == null)
                ViewBag.Session = null;
            else
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();

            return View(list);
        }

        // calling add page action
        [HttpGet]
        [Route("/new-list")]
        public async Task<IActionResult> Add()
        {
            return View("Add");
        }

        
        // Adding new list into database action
        [HttpPost]
        public async Task<IActionResult> AddList([FromForm] AddListModel addList)
        {
            var newList = new TaskList()
            {
                Id = Guid.NewGuid(),
                ListTitle = addList.ListTitle,
                UserId = new Guid(HttpContext.Session.GetString("UserId"))
            };

            await context.Lists.AddAsync(newList);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Delete the list
        [HttpGet]
        [Route("/delete-list/{id:guid}")]
        public async Task<IActionResult> DeleteList([FromRoute] Guid id)
        {
            var list = await context.Lists.FindAsync(id);

            if (list != null)
            {
                context.Lists.Remove(list);
                await context.SaveChangesAsync();

                return RedirectToAction( "Index" );
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/edit-list/{id:guid}")]
        public IActionResult Edit([FromRoute] Guid id)
        {
            var listData = (from list in context.Lists
                            where list.Id == id
                            select list).ToList();

            ViewBag.Id = id;
            ViewBag.Title = listData[0].ListTitle;

            return View("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> EditList(EditListModel editList)
        {

            var list = await context.Lists.FindAsync(editList.ListId);

            if (list != null)
            {
                list.ListTitle = editList.ListTitle;
                await context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        /*========= List Code End Here ========*/


        /*
         * Starting Tasks
         */

        // Adding new task into specific list
        [HttpGet]
        [Route("/{id:guid}")]
        public IActionResult AddTask(Guid id)
        {
            ViewBag.ListId = id;
            return View();
        }

        [HttpPost]
        [Route("/{id:guid}/add-task")]
        public IActionResult AddTask(AddTaskModels addTask)
        {
            var task = new Plan()
            {
                Task = addTask.Task,
                ListId = addTask.ListId
            };

            context.Tasks.Add(task);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Fetching all task according the list
        [HttpGet]
        [Route( "/{listId}/get-list" )]
        public IActionResult GetTask([FromRoute] string listId)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var tasks = context.Tasks.Where(t => t.ListId == new Guid(listId)).ToList();
                ViewBag.Tasks = tasks;
                return Json(new { data = tasks });
            }

            return Json(new
            {
                data = false
            });
            
        }

        // To check the status of task
        [HttpGet]
        [Route("/task-complete/{taskId:guid}")]
        public async Task<IActionResult> CompleteStatus([FromRoute] Guid taskId)
        {
            var task = await context.Tasks.FindAsync(taskId);

            if (task != null)
            {
                if(!task.Status)
                {
                    task.Status = true;
                    await context.SaveChangesAsync();
                }
                else
                {
                    task.Status = false;
                    await context.SaveChangesAsync();
                }
                return Json(new { data = task });
            }

            return Json(new { data = task });
        }

        // Task delete
        [HttpGet]
        [Route("/delete-task/{taskId:guid}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid taskid)
        {
            var task = await context.Tasks.FindAsync(taskid);
            if (task != null)
            {
                context.Tasks.Remove(task);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
