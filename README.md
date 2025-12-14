# People Management Application

**גרסה:** 1.0  
**מחברת:** Ayala Segal

---

## 1. דרישות הפעלה
- Windows עם Visual Studio 2019/2022
- SQL Server (LocalDB או Server רגיל) עם בסיס נתונים מוכן לפי הסקריפט המצורף `CreatePeopleTable.sql`
- פרויקט ASP.NET MVC (Framework 4.8)

---

## 2. יצירת הטבלה והנתונים הראשוניים
- להריץ את הקובץ `CreatePeopleTable.sql` ב-SQL Server
- הקובץ יוצר את הטבלה `People` ומוסיף 5 רשומות דוגמה
- שלב זה מבטיח שהפרויקט ירוץ עם נתונים ראשוניים
---

## 3. הרצת הפרויקט
1. פתחי את הקובץ `PeopleApp.sln` ב-Visual Studio
2. ודאי שה-Database מחובר כראוי (בדקי את ה-Connection string ב-`Web.config`)
3. לחצי על **F5** או **Start Debugging** → הפרויקט ירוץ בדפדפן דרך IIS Express

---

## 4. שימוש באפליקציה
- **הוספת משתמש חדש:**
  1. לחצי על "Add New Person"
  2. מלאי את השדות: Full Name, Phone, Email, Profile Image
  3. לחצי **Save**
  4. אם תכניסי אימייל לא חוקי, תופיע הודעת שגיאה

- **חיפוש אנשים:**
  1. הקלידי שם בשדה החיפוש
  2. לחצי **Search**
  3. כדי להציג את כל הרשימה מחדש, לחצי **Reset**

- **ייצוא PDF:**
  1. לחצי על **Export PDF**
  2. קובץ PDF נפתח בדפדפן (ניתן לשמור דרך הדפדפן או תוכנה לקריאת PDF)

---

## 5. מיקום קובץ PDF
- הקובץ נוצר על בסיס הרשימה הנוכחית של האנשים
- מוצג בדפדפן מיד לאחר לחיצה על **Export PDF**
---

## הסבר קצר

1. ודאו שחיבור למסד הנתונים מוגדר בפרויקט דרך ה-`Connection string` בקובץ `Web.config`.
2. פתחו את הקובץ `PeopleApp.sln` ב-Visual Studio.
3. הפעלת הפרויקט: לחצו על F5 → הפרויקט ירוץ בדפדפן דרך IIS Express.
4. מיקום קובץ ה-PDF: הקובץ נוצר בעת לחיצה על כפתור **Export PDF** וניתן לצפות בו ישירות בדפדפן או לשמור למחשב.
