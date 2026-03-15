# تخزين لينكات فيديوهات Google Drive في الداتابيز

## 1) تجهيز اللينك من Google Drive

1. ارفع الفيديو على **Google Drive**.
2. افتح الملف → **Right‑click** → **Share** (أو "مشاركة").
3. اختَر **"Anyone with the link"** (أي شخص لديه الرابط) عشان اللينك يشتغل بدون تسجيل.
4. انسخ الرابط. الصيغ المناسبة:
   - **للعرض في المتصفح:**  
     `https://drive.google.com/file/d/{FILE_ID}/view?usp=sharing`
   - **للتضمين (embed) في التطبيق:**  
     `https://drive.google.com/file/d/{FILE_ID}/preview`

استخدم **preview** لو التطبيق يعرض الفيديو داخل الصفحة (مثلاً في iframe أو WebView).

---

## 2) تخزين اللينك في الداتابيز

عندك طريقتين:

### أ) عبر الـ API (يحتاج يوزر Admin)

1. تأكد إن عندك **يوزر بدور Admin** (في جدول Users عمود `Role = 'Admin'`).
2. اعمل **Login** بنفس اليوزر ده وانسخ الـ **token**.
3. استدعي الـ API:

```http
PATCH https://your-api/api/Admin/statues/{statueId}/video
Authorization: Bearer YOUR_ADMIN_TOKEN
Content-Type: application/json

{
  "videoUrl": "https://drive.google.com/file/d/YOUR_FILE_ID/preview"
}
```

- **statueId:** الـ Id بتاع التمثال في جدول Statues (من الـ seed أو من نتيجة البحث).
- **videoUrl:** اللينك اللي نسخته من درايف (يفضّل لا يزيد عن 500 حرف).

أمثلة لـ statueId من الـ seed:

| التمثال      | statueId |
|-------------|----------|
| أخناتون     | 11111111-1111-1111-1111-111111111111 |
| نفرتيتي     | 22222222-2222-2222-2222-222222222222 |
| رمسيس الثاني | 33333333-3333-3333-3333-333333333333 |
| توت عنخ آمون | 55555555-5555-5555-5555-555555550001 |

في ملف **GPMuseumify.http** في المشروع موجود أمثلة جاهزة للطلب ده (قسم Admin).

### ب) عبر SQL مباشرة

لو مش عندك يوزر Admin أو عايزة تحدثي من SQL Server:

```sql
UPDATE Statues
SET VideoUrl = N'https://drive.google.com/file/d/YOUR_FILE_ID/preview',
    UpdatedAt = GETUTCDATE()
WHERE Id = '11111111-1111-1111-1111-111111111111';
```

غيّري `YOUR_FILE_ID` و `Id` حسب التمثال اللي عايزة تحطي له الفيديو.

---

## 3) التأكد إن اللينك اتخزن

- من الـ API: اعمل **Search** على التمثال (مثلاً "akhenaten") وافتح تفاصيله؛ هتشوفي حقل **videoUrl** باللينك الجديد.
- من الداتابيز: استعلمي على جدول **Statues** وعمود **VideoUrl** للـ Id المناسب.

---

## ملاحظات

- عمود **VideoUrl** في Statues طوله **500 حرف**. لو اللينك أطول، إما تقصّيه (مثلاً استخدام صيغة قصيرة) أو تزودي الطول من الـ migration.
- لو التطبيق يعرض الفيديو في iframe، استخدمي صيغة **/preview** من درايف.
- الـ API للتحديث محمي بـ **AdminOnly**؛ لازم الـ token يكون ليّوزر بدور Admin.
