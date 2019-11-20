using Microsoft.AspNet.Identity;
using SportProf.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using PagedList;
using System.Globalization;
using System;
using SportProf.Filters;

namespace SportProf.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsoluteUri;
            List<string> cultures = new List<string>() { "en", "ru", "uk" };
            if (!cultures.Contains(lang))
            {
                lang = "en";
            }
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        #region Main pages
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Competitions(int? page)
        {
            var id = User.Identity.GetUserId();
            if (id != null && id != "")
            {
                if (db.Users.FirstOrDefault(x => x.Id == id) != null && db.Users.FirstOrDefault(x => x.Id == id).Messages.Count != 0)
                {
                    ViewBag.Messages = db.Users.FirstOrDefault(x => x.Id == id).Messages.ToList();
                    db.Users.FirstOrDefault(x => x.Id == id).Messages.Clear();
                    await db.SaveChangesAsync();
                }
            }

            Dictionary<int, bool> res = new Dictionary<int, bool>();

            bool b;
            foreach (Competition comp in db.Competitions.ToList())
            {
                b = false;
                foreach (Request req in db.Requests.Where(x => x.CompetitionId == comp.Id).ToList())
                {
                    if (id != "" && db.Requests.Where(x => x.ApplicationUserId == id && x.CompetitionId == comp.Id).Count() == 1)
                    {
                        b = true;
                        break;
                    }
                }
                res.Add(comp.Id, b);
            }
            ViewBag.Requests = res;

            var competition = db.Competitions.Include(c => c.CompetitionType).ToList();

            return View(competition.ToPagedList(page ?? 1, 6));
        }
        [Authorize]
        public async Task<ActionResult> Manage(int? page)
        {
            var id = User.Identity.GetUserId();
            if (id != "")
            {
                if (db.Users.FirstOrDefault(x => x.Id == id) != null && db.Users.FirstOrDefault(x => x.Id == id).Messages.Count != 0)
                {
                    ViewBag.Messages = db.Users.FirstOrDefault(x => x.Id == id).Messages.ToList();
                    db.Users.FirstOrDefault(x => x.Id == id).Messages.Clear();
                    await db.SaveChangesAsync();
                }
            }

            var competitions = db.Competitions.Include(c => c.CompetitionType).Where(x => x.ApplicationUserId == id).ToList();
            return View(competitions.ToPagedList(page ?? 1, 6));
        }
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Users(int? page)
        {
            var id = User.Identity.GetUserId();
            if (id != "")
            {
                if (db.Users.FirstOrDefault(x => x.Id == id) != null && db.Users.FirstOrDefault(x => x.Id == id).Messages.Count != 0)
                {
                    ViewBag.Messages = db.Users.FirstOrDefault(x => x.Id == id).Messages.ToList();
                    db.Users.FirstOrDefault(x => x.Id == id).Messages.Clear();
                    await db.SaveChangesAsync();
                }
            }
            return View(db.Users.ToList().ToPagedList(page ?? 1, 8));
        }
        #endregion

        #region Competition
        [Authorize]
        public async Task<ActionResult> Create()
        {
            var id = User.Identity.GetUserId();
            if (id != "")
            {
                if (db.Users.FirstOrDefault(x => x.Id == id) != null && db.Users.FirstOrDefault(x => x.Id == id).Messages.Count != 0)
                {
                    ViewBag.Messages = db.Users.FirstOrDefault(x => x.Id == id).Messages.ToList();
                    db.Users.FirstOrDefault(x => x.Id == id).Messages.Clear();
                    await db.SaveChangesAsync();
                }
            }

            List<SelectListItem> types = new List<SelectListItem>();
            foreach (var type in db.CompetitionTypes.ToList())
            {
                types.Add(new SelectListItem { Text = type.Name, Value = type.Name });
            }
            ViewBag.CompetitionTypes = types;

            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(string name, string description, string type, string red)
        {
            CompetitionType ctype = db.CompetitionTypes.ToList().FindLast(x => x.Name == type);
            string id = this.User.Identity.GetUserId();
            var applicationUser = db.Users.FirstOrDefault(x => x.Id == id);
            int i = 1 + db.Users.ToList().IndexOf(applicationUser);
            if (i < 1)
            {
                var ermessages = db.Users.FirstOrDefault(x => x.Id == id).Messages;
                ermessages.Add(new Message { Subject = Resources.Messages.Error + "!", Body = Resources.Messages.CompCreateFailed, Type = "alert-danger" });
                await db.SaveChangesAsync();

                switch (red)
                {
                    case "manage":
                        return RedirectToAction("Manage", "Home");
                    case "competitions":
                        return RedirectToAction("Competitions", "Home");
                    default:
                        return RedirectToAction("", "Home");
                }
            }

            db.Competitions.Add(new Competition { Name = name, Description = description, CompetitionType = ctype, CompetitionTypeId = ctype.Id, ApplicationUser = applicationUser, ApplicationUserId = applicationUser.Id });
            var messages = db.Users.FirstOrDefault(x => x.Id == id).Messages;
            messages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.CompCreateSuccess, Type = "alert-success" });
            await db.SaveChangesAsync();

            switch (red)
            {
                case "manage":
                    return RedirectToAction("Manage", "Home", new { alert = "crcomp" });
                case "competitions":
                    return RedirectToAction("Competitions", "Home", new { alert = "crcomp" });
                default:
                    return RedirectToAction("", "Home");
            }
        }
        [Authorize]
        public async Task<ActionResult> Remove(int? id, string red)
        {
            if (id == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }
            var uid = this.User.Identity.GetUserId();
            string user = db.Competitions.FirstOrDefault(x => x.Id == id).ApplicationUser.UserName;
            if (!User.IsInRole("admin") && User.Identity.Name != user)
            {
                var ermessages = db.Users.FirstOrDefault(x => x.Id == uid).Messages;
                ermessages.Add(new Message { Subject = Resources.Messages.Error + "!", Body = Resources.Messages.CompDelFailed, Type = "alert-danger" });
                await db.SaveChangesAsync();

                switch (red)
                {
                    case "manage":
                        return RedirectToAction("Manage", "Home");
                    case "competitions":
                        return RedirectToAction("Competitions", "Home");
                    default:
                        return RedirectToAction("", "Home");
                }
            }

            db.UserInCompetitions.RemoveRange(db.UserInCompetitions.Where(x => x.Location.CompetitionId == id));
            db.Locations.RemoveRange(db.Locations.Where(x => x.CompetitionId == id));
            db.Competitions.Remove(db.Competitions.FirstOrDefault(x => x.Id == id));
            var messages = db.Users.FirstOrDefault(x => x.Id == uid).Messages;
            messages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.CompDelSuccess, Type = "alert-success" });
            await db.SaveChangesAsync();

            switch (red)
            {
                case "manage":
                    return RedirectToAction("Manage", "Home");
                case "competitions":
                    return RedirectToAction("Competitions", "Home");
                default:
                    return RedirectToAction("", "Home");
            }
        }
        [Authorize]
        public async Task<ActionResult> Edit(int? page, int? cid)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }

            ViewBag.Locations = db.Locations.Where(x => x.CompetitionId == cid).ToList();


            List<SelectListItem> types = new List<SelectListItem>();
            foreach (var type in db.CompetitionTypes.ToList())
            {
                types.Add(new SelectListItem { Text = type.Name, Value = type.Name });
                if (db.Competitions.FirstOrDefault(x => x.Id == cid).CompetitionTypeId == type.Id) types.Last().Selected = true;
            }
            ViewBag.CompetitionTypes = types;


            var userid = this.User.Identity.GetUserId();
            ViewBag.Count = db.Requests.Where(x => x.Accepted == false && x.CompetitionId == cid).Count();
            ViewBag.Users = db.Users.Where(x => db.Requests.Where(y => y.CompetitionId == cid && y.ApplicationUserId == x.Id && y.Accepted == true).Count() > 0).ToList().ToPagedList(page ?? 1, 7);
            ViewBag.UsersComp = db.UserInCompetitions.Where(x => x.Location.CompetitionId == cid).OrderBy(x => x.Location.Name).ToList().ToPagedList(page ?? 1, 7);

            if (userid != "")
            {
                if (db.Users.FirstOrDefault(x => x.Id == userid) != null && db.Users.FirstOrDefault(x => x.Id == userid).Messages.Count != 0)
                {
                    ViewBag.Messages = db.Users.FirstOrDefault(x => x.Id == userid).Messages.ToList();
                    db.Users.FirstOrDefault(x => x.Id == userid).Messages.Clear();
                    await db.SaveChangesAsync();
                }
            }

            var competition = db.Competitions.FirstOrDefault(x => x.Id == cid);
            return View(competition);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(int cid, string name, string description, string type, int? page)
        {
            var comp = db.Competitions.FirstOrDefault(x => x.Id == cid);
            comp.Name = name;
            comp.Description = description;
            CompetitionType t = db.CompetitionTypes.FirstOrDefault(x => x.Name == type);
            comp.CompetitionType = t;
            comp.CompetitionTypeId = t.Id;
            await db.SaveChangesAsync();

            List<Competition> comps = db.Competitions.ToList();

            var userid = this.User.Identity.GetUserId();
            var ermessages = db.Users.FirstOrDefault(x => x.Id == userid).Messages;
            ermessages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.Saved, Type = "alert-success" });

            await db.SaveChangesAsync();

            return await Edit(page, cid);
        }
        public async Task<ActionResult> Info(int? page, int? cid, string red)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }

            ViewBag.Locations = db.Locations.Where(x => x.CompetitionId == cid).ToList();

            var userid = this.User.Identity.GetUserId();
            ViewBag.UsersComp = db.UserInCompetitions.Where(x => x.Location.CompetitionId == cid).OrderBy(x => x.Location.Name).ToList().ToPagedList(page ?? 1, 10);

            if (userid != "")
            {
                if (db.Users.FirstOrDefault(x => x.Id == userid) != null && db.Users.FirstOrDefault(x => x.Id == userid).Messages.Count != 0)
                {
                    ViewBag.Messages = db.Users.FirstOrDefault(x => x.Id == userid).Messages.ToList();
                    db.Users.FirstOrDefault(x => x.Id == userid).Messages.Clear();
                    await db.SaveChangesAsync();
                }
            }

            var competition = db.Competitions.FirstOrDefault(x => x.Id == cid);
            return View(competition);
        }
        [Authorize]
        public ActionResult AddLocation(int? cid)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }

            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddLocation(int cid, string name, int userCount, string red)
        {
            db.Locations.Add(new Location { Name = name, UserCount = userCount, CompetitionId = cid, Competition = db.Competitions.FirstOrDefault(x => x.Id == cid) });

            var id = this.User.Identity.GetUserId();
            var ermessages = db.Users.FirstOrDefault(x => x.Id == id).Messages;
            ermessages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.LocAddSuccess, Type = "alert-success" });

            await db.SaveChangesAsync();

            return RedirectToAction("Edit", "Home", new { cid, red });
        }
        [Authorize]
        public async Task<ActionResult> RemoveLocation(int? cid, int id, string red)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }

            db.Locations.Remove(db.Locations.FirstOrDefault(x => x.Id == id));
            var uid = this.User.Identity.GetUserId();
            var ermessages = db.Users.FirstOrDefault(x => x.Id == uid).Messages;
            ermessages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.LocDelSuccess, Type = "alert-success" });
            await db.SaveChangesAsync();

            return RedirectToAction("Edit", "Home", new { cid, red });
        }
        [Authorize]
        public ActionResult Watch(int? page, int? cid)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }

            var userInCompetition = db.UserInCompetitions.Where(x => x.Location.CompetitionId == cid).ToList();
            return View(userInCompetition.ToPagedList(page ?? 1, 6));
        }
        public async Task<ActionResult> Result(int? page, int? cid)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }
            ViewBag.Status = db.Competitions.FirstOrDefault(x => x.Id == cid).Status == Status.Start;
            var userid = this.User.Identity.GetUserId();
            if (userid != "")
            {
                if (db.Users.FirstOrDefault(x => x.Id == userid) != null && db.Users.FirstOrDefault(x => x.Id == userid).Messages.Count != 0)
                {
                    ViewBag.Messages = db.Users.FirstOrDefault(x => x.Id == userid).Messages.ToList();
                    db.Users.FirstOrDefault(x => x.Id == userid).Messages.Clear();
                    await db.SaveChangesAsync();
                }
            }

            var userInCompetition = db.UserInCompetitions.Where(x => x.Location.CompetitionId == cid).ToList();
            return View(userInCompetition.ToPagedList(page ?? 1, 8));
        }
        #endregion

        #region Requests
        [Authorize]
        public ActionResult SendRequest(int? id)
        {
            if (id == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }

            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SendRequest(int id, string name, string description, int competitionId)
        {
            string userid = this.User.Identity.GetUserId();
            var applicationUser = db.Users.FirstOrDefault(x => x.Id == userid);
            int i = 1 + db.Users.ToList().IndexOf(applicationUser);
            if (i < 1)
            {
                var ermessages = db.Users.FirstOrDefault(x => x.Id == userid).Messages;
                ermessages.Add(new Message { Subject = Resources.Messages.Error + "!", Body = Resources.Messages.RequestFailed, Type = "alert-danger" });
                return RedirectToAction("Competitions", "Home");
            }
            db.Requests.Add(new Request { Name = name, Description = description, CompetitionId = competitionId, Competition = db.Competitions.FirstOrDefault(x => x.Id == competitionId), ApplicationUserId = applicationUser.Id, ApplicationUser = applicationUser });

            List<Request> req = db.Requests.ToList();

            var messages = db.Users.FirstOrDefault(x => x.Id == userid).Messages;
            messages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.RequestSuccess, Type = "alert-success" });
            await db.SaveChangesAsync();

            return RedirectToAction("Competitions", "Home");
        }
        [Authorize]
        public async Task<ActionResult> Requests(int? page, int? cid)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }

            var id = User.Identity.GetUserId();
            if (id != "")
            {
                if (db.Users.FirstOrDefault(x => x.Id == id) != null && db.Users.FirstOrDefault(x => x.Id == id).Messages.Count != 0)
                {
                    ViewBag.Messages = db.Users.FirstOrDefault(x => x.Id == id).Messages.ToList();
                    db.Users.FirstOrDefault(x => x.Id == id).Messages.Clear();
                    await db.SaveChangesAsync();
                }
            }

            var requests = db.Requests.Where(x => x.CompetitionId == cid && x.Accepted == false).Include(c => c.ApplicationUser).ToList();
            return View(requests.ToPagedList(page ?? 1, 6));
        }
        [Authorize]
        public async Task<ActionResult> AcceptRequest(int? cid, int id, string red)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }
            var el = db.Requests.FirstOrDefault(x => x.Id == id && x.CompetitionId == cid);
            el.Accepted = true;

            string uid = this.User.Identity.GetUserId();
            var ermessages = db.Users.FirstOrDefault(x => x.Id == uid).Messages;
            ermessages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.AcceptSuccess, Type = "alert-success" });

            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "Home", new { cid, red });
        }
        [Authorize]
        public async Task<ActionResult> RemoveCurUserRequest(int? cid)
        {
            string uid = User.Identity.GetUserId();
            await RemoveRequest(cid, db.Requests.FirstOrDefault(x => x.ApplicationUserId == uid && x.CompetitionId == cid).Id, "competitions");
            return RedirectToAction("competitions", "Home");
        }
        [Authorize]
        public async Task<ActionResult> RemoveRequest(int? cid, int id, string red)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }
            db.Requests.Remove(db.Requests.FirstOrDefault(x => x.Id == id && x.CompetitionId == cid));

            string uid = this.User.Identity.GetUserId();
            var ermessages = db.Users.FirstOrDefault(x => x.Id == uid).Messages;
            ermessages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.DeclineSuccess, Type = "alert-success" });

            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "Home", new { cid, red });
        }
        [Authorize]
        public async Task<ActionResult> RemoveUserRequest(int? cid, string id, string red)
        {
            if (cid == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }
            var el = db.Requests.FirstOrDefault(x => x.ApplicationUserId == id && x.CompetitionId == cid);
            el.Accepted = false;

            string uid = this.User.Identity.GetUserId();
            var ermessages = db.Users.FirstOrDefault(x => x.Id == uid).Messages;
            ermessages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.RequestDelSuccess, Type = "alert-success" });

            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "Home", new { cid, red });
        }
        #endregion

        #region Competition process
        [Authorize]
        public async Task<ActionResult> EndRequests(int? id)
        {
            if (id == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }

            db.Competitions.FirstOrDefault(x => x.Id == id).Status = Status.Prepare;

            List<Request> reqs = db.Requests.Where(x => x.CompetitionId == id && x.Accepted == true).OrderByDescending(x => x.ApplicationUser.CompetitionCount).ToList();


            Dictionary<int, bool> locCount = new Dictionary<int, bool>();
            foreach (var item in db.Locations.Where(x => x.CompetitionId == id).ToList())
            {
                locCount.Add(item.Id, false);
            }

            foreach (Request item in reqs)//перебираем пользователей в запросах
            {
                foreach (Location loc in db.Locations.Where(x => x.CompetitionId == id).ToList())
                {
                    int con = db.UserInCompetitions.Where(x => x.LocationId == loc.Id).ToList().Count();
                    if (!locCount[loc.Id] && con < loc.UserCount) //если в локации достаточно места, то закидываем пользователя
                    {
                        db.UserInCompetitions.Add(new UserInCompetition { LocationId = loc.Id, Location = loc, UserId = item.ApplicationUserId, User = db.Users.FirstOrDefault(x => x.Id == item.ApplicationUserId) });
                        var umessages = db.Users.FirstOrDefault(x => x.Id == item.ApplicationUserId).Messages;
                        umessages.Add(new Message { Subject = Resources.Messages.PrepareMain, Body = Resources.Messages.PrepareComp + " \"" + db.Competitions.FirstOrDefault(x => x.Id == id).Name + "\".\n" + Resources.Messages.PrepareLoc + " \"" + loc.Name + "\".", Type = "alert-success" });
                        await db.SaveChangesAsync();
                        if (item.ApplicationUser.EmailConfirmed)
                        {
                            EmailSendController.SendEmail(item.ApplicationUser.Email/*"jack991769@gmail.com"*/, Resources.Messages.PrepareMain, Resources.Messages.PrepareComp + " \"" + db.Competitions.FirstOrDefault(x => x.Id == id).Name + "\".\n" + Resources.Messages.PrepareLoc + " \"" + loc.Name + "\".");
                        }
                        break;
                    }
                    else
                    {
                        locCount[loc.Id] = true;
                    }

                    if (locCount.Where(x => x.Value == false).Count() == 0) //если места нет нигде => ошибка
                    {
                        var cuid = this.User.Identity.GetUserId();
                        var cmessages = db.Users.FirstOrDefault(x => x.Id == cuid).Messages;
                        cmessages.Add(new Message { Subject = Resources.Messages.Error + "!", Body = Resources.Messages.UserCountError, Type = "alert-danger" });
                        await db.SaveChangesAsync();
                        return RedirectToAction("Manage", "Home");
                    }
                }
            }

            db.Requests.RemoveRange(db.Requests.Where(x => x.CompetitionId == id));//???
            var uid = this.User.Identity.GetUserId();
            var messages = db.Users.FirstOrDefault(x => x.Id == uid).Messages;
            messages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.StatusPrepared });

            await db.SaveChangesAsync();

            return RedirectToAction("Manage", "Home");
        }
        [Authorize]
        public async Task<ActionResult> Start(int? id)
        {
            if (id == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }
            db.Competitions.FirstOrDefault(x => x.Id == id).Status = Status.Start;

            List<UserInCompetition> reqs = db.UserInCompetitions.Where(x => x.Location.CompetitionId == id).ToList();
            foreach (UserInCompetition item in reqs)
            {
                var umessages = db.Users.FirstOrDefault(x => x.Id == item.UserId).Messages;
                umessages.Add(new Message { Subject = Resources.Messages.CompStart, Body = Resources.Messages.Competition + " \"" + db.Competitions.FirstOrDefault(x => x.Id == id).Name + "\" is started.\n" + Resources.Messages.PrepareLoc + " \"" + item.Location.Name + "\".", Type = "alert-success" });
            }
            var uid = this.User.Identity.GetUserId();
            var messages = db.Users.FirstOrDefault(x => x.Id == uid).Messages;
            messages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.StatusStarted, Type = "alert-success" });

            await db.SaveChangesAsync();
            return RedirectToAction("Manage", "Home");
        }
        [Authorize]
        public async Task<ActionResult> StopCompetition(int? id)
        {
            if (id == null)
            {
                throw new HttpException(404, "Are you sure you're in the right place?");
            }
            db.Competitions.FirstOrDefault(x => x.Id == id).Status = Status.End;

            List<UserInCompetition> reqs = db.UserInCompetitions.Where(x => x.Location.CompetitionId == id).ToList();
            foreach (UserInCompetition item in reqs)
            {
                var umessages = db.Users.FirstOrDefault(x => x.Id == item.UserId).Messages;
                umessages.Add(new Message { Subject = Resources.Messages.CompEnded, Body = Resources.Messages.Competition + " \"" + db.Competitions.FirstOrDefault(x => x.Id == id).Name + "\" " + Resources.Messages.isEnd + ".\n" + Resources.Messages.YourRes + " \"" + item.Result + "\".", Type = "alert-success" });
                if (item.User.EmailConfirmed)
                {
                    EmailSendController.SendEmail(item.User.Email/*"jack991769@gmail.com"*/, Resources.Messages.CompEnded, Resources.Messages.Competition + " \"" + db.Competitions.FirstOrDefault(x => x.Id == id).Name + "\" " + Resources.Messages.isEnd + ".\n" + Resources.Messages.YourRes + " \"" + item.Result + "\".");
                }
                db.Users.FirstOrDefault(x => x.Id == item.UserId).CompetitionCount += 1;
            }
            var uid = this.User.Identity.GetUserId();
            var messages = db.Users.FirstOrDefault(x => x.Id == uid).Messages;
            messages.Add(new Message { Subject = Resources.Messages.Success + "!", Body = Resources.Messages.StatusEnded, Type = "alert-success" });

            await db.SaveChangesAsync();
            return RedirectToAction("Manage", "Home");
        }
        public void Save(string id, string result)
        {
            List<UserInCompetition> uc = db.UserInCompetitions.ToList();
            int i = int.Parse(id);
            int res = int.Parse(result);
            db.UserInCompetitions.FirstOrDefault(x => x.Id == i).Result = res;
            db.SaveChanges();
        }
        #endregion

        [HttpPost]
        public void SendPulse(int cid, string email, int pulse)
        {
            db.UserInCompetitions.FirstOrDefault(x => x.User.Email == email && x.Location.CompetitionId == cid).Pulse = pulse;
            db.SaveChanges();
        }
    }
}
//уведомления при пустых таблицах
//Competitions
//Manage
//Users
//Edit -> Locations
//Edit -> Acepted requests
//Requests
//Watch
//Info
//Result

//ограничение по выводу данных на экран
//ограничения запросов
//регистрация и вход(login != email, phone, google add)


//типы соревнований в виде файла или админки
//переходы на стрелочки?!