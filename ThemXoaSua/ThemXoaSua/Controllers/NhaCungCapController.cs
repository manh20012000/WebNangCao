using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThemXoaSua.Models;

namespace ThemXoaSua.Controllers
{
    public class NhaCungCapController : Controller
    {
        // GET: NhaCungCap
        private Models.DHEntities2 db = new Models.DHEntities2();
           
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            DHEntities2 db = new DHEntities2();
            List<NHACUNGCAP> ncc = db.NHACUNGCAPs.ToList();
            return View(ncc);
        }
        public ActionResult CreateNCC()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateNCC (NHACUNGCAP ncc)
        {
            DHEntities2 db = new DHEntities2();
            db.NHACUNGCAPs.Add(ncc);
           //luu lai thay doi 
            db.SaveChanges();  
            return RedirectToAction("Home");
        }


        public ActionResult Edit (string id)
        {
            //Tim doi tuong theo mancc
            var obj = db.NHACUNGCAPs.Find(id);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Edit (NHACUNGCAP obj)
        {
            try
            {
                var edncc = db.NHACUNGCAPs.Find(obj.MANCC);
                edncc.TENNCC = obj.TENNCC;
                edncc.DIACHI = obj.DIACHI;
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Home");
        }
        public ActionResult Delete(string id)
        {
            //Tim doi tuong theo mancc
            var obj = db.NHACUNGCAPs.Find(id);
            return View(obj);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteNCC(string id)
        {
            try
            {
                var edncc = db.NHACUNGCAPs.Find(id);
                if(edncc!=null)
                {
                    db.NHACUNGCAPs.Remove(edncc); 
                    db.SaveChanges();
                }
              
            }
            catch
            {

            }
            return RedirectToAction("Home");
        }
    }
}