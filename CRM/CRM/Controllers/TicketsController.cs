using CRM.Models;
using CRM.Models.Abstract;
using CRM.Models.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CRM.Controllers
{
    public class TicketsController : Controller
    {
        IRepository repository;
        public TicketsController(IRepository repo)
        {
            repository = repo;
        }
        //
        // GET: Tickets
        
        public string FileUpload()
        {
            int iUploadedCnt = 0;
            int id = 1;
            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            id = Convert.ToInt32(System.Web.HttpContext.Current.Request["ticketId"]);
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                byte[] imageData = null;
                System.Web.HttpPostedFileBase hpf = Request.Files[iCnt];
                if (hpf.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(hpf.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(hpf.ContentLength);
                    }
                    //IQueryable<Images> images = repository.Images.Where(x => x.ticket_id == id);
                    Images image = new Images { ticket_id = id, ImageData = imageData };
                    if (repository.CreateRecord<Images>(image) != null)
                        return iUploadedCnt + " Files Uploaded Successfully";
                }
            }
            return "Upload Failed";
        }
        private List<ViewTickets> createViewTicketsModel(IQueryable<Tickets> tickets, int level)
        {
            List<ViewTickets> list = new List<ViewTickets>();
            foreach (Tickets ticket in tickets)
                list.Add(
                    new ViewTickets
                    {
                        id = ticket.id,
                        parent_id = ticket.parent_id,
                        manager_id = ticket.manager_id,
                        body = ticket.body,
                        department_id = ticket.department_id,
                        cust_id = ticket.cust_id,
                        email = ticket.email,
                        mail_data = ticket.mail_data,
                        reference = ticket.reference,
                        responce = ticket.responce,
                        status_id = ticket.status_id,
                        subject = ticket.subject,
                        task_id = ticket.task_id,
                        user_id = ticket.user_id,
                        level = level
                    }
                    );
            return list;
        }
        private List<ViewTickets> createTicketChain(int? ticketId, ref List<ViewTickets> ticketsChain, ref int level)
        {
            IQueryable<Tickets> tickets = repository.Tickets.Where(x => x.parent_id == ticketId);
            List<ViewTickets> chain = createViewTicketsModel(tickets, level);
            foreach (ViewTickets chainTicket in chain)
            {
                ticketsChain.Add(chainTicket);
            }
            level++;
            foreach (Tickets ticket in tickets)
            {
                createTicketChain(ticket.id, ref ticketsChain, ref level);
            }
            level--;
            return ticketsChain;
        }
        private void deleteTickets(int ticketId)
        {
            IQueryable<Tickets> tickets = repository.Tickets.Where(x => x.parent_id == ticketId);
            if (tickets.Count() > 0)
            {
                foreach (Tickets ticket in tickets)
                {
                    deleteTickets(ticket.id);
                    IQueryable<Images> images = repository.Images.Where(x => x.id == ticket.id);
                    foreach (Images image in images)
                        repository.DeleteRecord<Images>(image);
                    repository.DeleteRecord<Tickets>(ticket);
                }
            }
            
            repository.DeleteRecord<Tickets>(repository.Tickets.FirstOrDefault(x => x.id == ticketId));
        }
        public JsonResult SaveTicket(string ticket)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ViewTickets temp = jsonSerializer.Deserialize<ViewTickets>(ticket);
            Tickets t;
            if (temp.id == -1)
            {
                t = new Tickets
                {
                    id = temp.id,
                    department_id = temp.department_id,
                    user_id = temp.user_id,
                    status_id = temp.status_id,
                    email = temp.email,
                    subject = temp.subject,
                    body = temp.body,
                    mail_data = temp.mail_data,
                    reference = temp.reference,
                    responce = temp.responce,
                    parent_id = temp.parent_id,
                    task_id = temp.task_id,
                    manager_id = temp.manager_id,
                    cust_id = temp.manager_id
                };
            }
            else
            {
                t = repository.Tickets.FirstOrDefault(x => x.id == temp.id);
                t.id = temp.id;
                t.department_id = temp.department_id;
                t.user_id = temp.user_id;
                t.status_id = temp.status_id;
                t.email = temp.email;
                t.subject = temp.subject;
                t.body = temp.body;
                t.mail_data = temp.mail_data;
                t.reference = temp.reference;
                t.responce = temp.responce;
                t.parent_id = temp.parent_id;
                t.task_id = temp.task_id;
                t.manager_id = temp.manager_id;
                t.cust_id = temp.manager_id;
            }
            
            if (t.id == -1)
                t = repository.CreateRecord<Tickets>(t);
            else
                repository.UpdateRecord<Tickets>(t);
            return Json(t.id.ToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTicket(int? ticketId)
        {
            try
            {
                //repository.DbContext.delete_images(ticketId);
                //repository.SaveChanges<Images>();
                ObjectParameter id = new ObjectParameter("return_ticket", typeof(int));
                repository.DbContext.delete_tickets(ticketId, id);
                //repository.SaveChanges<Tickets>();
            }
            catch (Exception ex)
            {
                return Json(ex.Data.ToString(), JsonRequestBehavior.AllowGet);
            }
            
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteImage(int? imageId)
        {
            repository.DeleteRecord<Images>(repository.Images.FirstOrDefault(x=>x.id == imageId));
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadImage(int ticketId)
        {
            IQueryable<Images> images = repository.Images.Where(x => x.ticket_id == ticketId);
            string[,] retImages;
            if (images.Count() > 0)
            {
                retImages = new string[images.Count(), 2];
                int iCount = 0;
                foreach (var image in images)
                {
                    string str = System.Convert.ToBase64String(image.ImageData, 0, image.ImageData.Length);
                    retImages[iCount, 0] = str;
                    retImages[iCount, 1] = image.id.ToString();
                    iCount++;

                }
                return Json(retImages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            
        }
        public JsonResult GetTicketsChain(int ticketId)
        {
            List<ViewTickets> ticketsChain =  createViewTicketsModel(repository.Tickets.Where(x => x.id == ticketId),0);
            int level = 1;
            createTicketChain(ticketId, ref ticketsChain, ref level);
            return Json(ticketsChain.OrderBy(x=>x.level), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewTickets()
        {
            TicketsModel ticketsModel = new TicketsModel();
            CommonData commonData = new CommonData(repository);
            ticketsModel.tasks = commonData.GetTasks();
            ticketsModel.statuses = commonData.GetStatuses();
            ticketsModel.users = commonData.GetUsers();
            ticketsModel.managers = commonData.GetManagers();
            ticketsModel.customers = commonData.GetCustomers();
            if (!User.IsInRole("Customer"))
                ticketsModel.tickets = createViewTicketsModel(repository.Tickets.Where(x => x.parent_id == null), 0);
            else
            {
                Users user = repository.Users.FirstOrDefault(x => x.name == User.Identity.Name);
                ticketsModel.tickets = createViewTicketsModel(repository.Tickets.Where(x => x.parent_id == null && x.manager_id == user.id), 0);
            }
                
            //int user_id 
                
            return View(ticketsModel);
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}