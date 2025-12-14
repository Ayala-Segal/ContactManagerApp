using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using מטלת_בית.Models;

namespace מטלת_בית.Controllers
{
    public class PeopleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: People
        public ActionResult Index(string searchString)
        {
            var people = db.People.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                people = people.Where(p => p.FullName.Contains(searchString));
            }

            people = people.OrderBy(p => p.FullName);

            return View(people.ToList());
        }

        // GET: People/ExportPdf
        public ActionResult ExportPdf()
        {
            var people = db.People.OrderBy(p => p.FullName).ToList();

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // --- Load Hebrew/English Font ---
                string fontPath = Server.MapPath("~/Content/font/arial.ttf");
                BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(bf, 12);
                Font titleFont = new Font(bf, 16, Font.BOLD);

                // --- Title ---
                Paragraph title = new Paragraph("People List", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 15;
                doc.Add(title);

                // --- Table: 4 columns (Full Name, Phone, Email, Image) ---
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 3f, 2f, 3f, 2f }); 

                // --- Header Row (English) ---
                BaseColor headerColor = new BaseColor(230, 230, 230);
                table.AddCell(new PdfPCell(new Phrase("Full Name", font)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Phone", font)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Email", font)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Image", font)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_CENTER });

                // --- Rows ---
                foreach (var p in people)
                {
                    // Full Name
                    table.AddCell(CreateCenteredCell(p.FullName, font));

                    // Phone
                    table.AddCell(CreateCenteredCell(p.Phone, font));

                    // Email
                    table.AddCell(CreateCenteredCell(p.Email, font));

                    // Image
                    PdfPCell imgCell = new PdfPCell();
                    imgCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    imgCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    imgCell.Padding = 5;

                    if (!string.IsNullOrEmpty(p.ImagePath))
                    {
                        try
                        {
                            string physicalPath = Server.MapPath(p.ImagePath);
                            if (System.IO.File.Exists(physicalPath))
                            {
                                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(physicalPath);
                                img.ScaleAbsolute(50, 50);
                                imgCell.AddElement(img);
                            }
                            else
                            {
                                imgCell.Phrase = new Phrase("-", font);
                            }
                        }
                        catch
                        {
                            imgCell.Phrase = new Phrase("-", font);
                        }
                    }
                    else
                    {
                        imgCell.Phrase = new Phrase("-", font);
                    }

                    table.AddCell(imgCell);
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(),
                    "application/pdf",
                    "PeopleList.pdf");
            }
        }

        // --- Function to create a cell with centered content ---
        private PdfPCell CreateCenteredCell(string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text ?? "", font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;

            // Check if the text is Hebrew
            if (!string.IsNullOrEmpty(text) && Regex.IsMatch(text, @"\p{IsHebrew}"))
            {
                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL; 
            }
            else
            {
                cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            }

            return cell;
        }

        // GET: People/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var person = db.People.Find(id);
            if (person == null)
                return HttpNotFound();

            return View(person);
        }
       
        // GET: People/Upsert
        public ActionResult Upsert(int? id)
        {
            if (id == null)
                return View(new Person()); //add

            var person = db.People.Find(id);
            if (person == null)
                return HttpNotFound();

            return View(person); //update
        }

        // POST: People/Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert(Person person, HttpPostedFileBase imageFile)
        {
            // Basic validations
            if (db.People.Any(p => p.Email == person.Email && p.Id != person.Id))
                ModelState.AddModelError("Email", "The email already exists in the system.");

            if (!string.IsNullOrEmpty(person.Phone) && !person.Phone.All(char.IsDigit))
                ModelState.AddModelError("Phone", "Phone number must contain only digits.");


            if (person.Id == 0 && (imageFile == null || imageFile.ContentLength == 0))
                ModelState.AddModelError("ImageFile", "Profile image is required.");

            if (!ModelState.IsValid)
                return View(person);

            // save image file
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                    var ext = Path.GetExtension(imageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(ext) || !allowedTypes.Contains(imageFile.ContentType))
                    {
                        ModelState.AddModelError("ImageFile", "Allowed file types: .jpg, .jpeg, .png, .gif");
                        return View(person);
                    }

                    if (imageFile.ContentLength > 3 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImageFile", "File size cannot exceed 3 MB.");
                        return View(person);
                    }

                // ---- SAVE FILE ----
                string fileName = Guid.NewGuid() + ext;
                string folder = "/Content/Uploads/";
                string path = Server.MapPath("~" + folder);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fullPath = Path.Combine(path, fileName);
                imageFile.SaveAs(fullPath);

                // save relative path in the DB
                person.ImagePath = folder + fileName;
            }


            if (person.Id == 0)
            {
                // add new record
                db.People.Add(person);
            }
            else
            {
                //update existing record
                var existing = db.People.Find(person.Id);
                if (existing == null) return HttpNotFound();

                existing.FullName = person.FullName;
                existing.Phone = person.Phone;
                existing.Email = person.Email;

                if (!string.IsNullOrEmpty(person.ImagePath))
                    existing.ImagePath = person.ImagePath;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        // GET: People/Delete
        public ActionResult Delete(int id)
        {
            var person = db.People.Find(id);
            if (person == null)
                return HttpNotFound();

            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
 

}

