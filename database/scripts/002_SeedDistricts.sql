-- ============================================
-- TUMS Phase 1: Seed Districts Data
-- Extracted from districts.json
-- ============================================

USE transportationDB;
GO

-- Insert Districts (unique names from all periods/buses)
-- Using MERGE to avoid duplicates

MERGE INTO [dbo].[Districts] AS target
USING (VALUES
    -- Core Districts extracted from districts.json
    (N'العزيزية مسلم'),
    (N'م . باقدو'),
    (N'م . فهد'),
    (N'الكردي'),
    (N'السيح'),
    (N'الدعيثة أبو مرخة'),
    (N'الجرف الغربي'),
    (N'الصالحية'),
    (N'العنابس'),
    (N'القبلتين'),
    (N'الغربية'),
    (N'الفيصلية'),
    (N'العنبرية'),
    (N'السقيا'),
    (N'المصانع'),
    (N'المستراح'),
    (N'السحمان'),
    (N'الروضة'),
    (N'الكويتية'),
    (N'العوالي'),
    (N'قربان'),
    (N'الخاتم'),
    (N'البحر'),
    (N'الأعمدة'),
    (N'شارع فلسطين'),
    (N'عروة'),
    (N'الوبرة'),
    (N'الرانوناء'),
    (N'شوران'),
    (N'الجرف الشرقي'),
    (N'العزيزية البخاري'),
    (N'الدعيثة'),
    (N'م طيبة'),
    (N'الدويمة'),
    (N'الظاهرة'),
    (N'العصبة'),
    (N'السديري'),
    (N'المغاربة'),
    (N'الروابي'),
    (N'مخطط الزهرة'),
    (N'مخطط الأمير نايف'),
    (N'القصواء'),
    (N'النصر'),
    (N'الحزام'),
    (N'الخليل'),
    (N'الشهداء'),
    (N'الطيارية'),
    (N'واحة السلام'),
    (N'الحرس'),
    (N'الشافية'),
    (N'الجمعة'),
    (N'النسيم'),
    (N'الأزهري'),
    (N'التلال'),
    (N'الشبيبة'),
    (N'الربوة'),
    (N'الخالدية'),
    (N'الإسكان الشمالي'),
    (N'المبعوث'),
    (N'مخطط الأمير تركي المطار'),
    (N'المغيسلة'),
    (N'الجبور'),
    (N'الزاهدية'),
    (N'الملك فهد'),
    (N'مهزور'),
    (N'الإسكان الجنوبي'),
    (N'الوعير'),
    (N'العريض'),
    (N'الناصرية'),
    (N'باقدو'),
    (N'م فهد'),
    (N'الدويخله'),
    (N'العزيزية الإمام البخاري'),
    (N'ابو مرخة'),
    (N'البركة'),
    (N'الهجرة'),
    (N'ابو بريقاء'),
    (N'حي الدفاع'),
    (N'شارع الملك عبدالعزيز'),
    (N'حي الخاتم'),
    (N'بلاد السديري'),
    (N'الأصيفرين'),
    (N'الإسكان'),
    (N'جمل الليل'),
    (N'سلطانة'),
    (N'العنابس (سكن الطلاب)'),
    (N'مخطط طيبة'),
    (N'العزيزية الإمام مسلم')
) AS source ([DistrictNameAr])
ON target.[DistrictNameAr] = source.[DistrictNameAr]
WHEN NOT MATCHED THEN
    INSERT ([DistrictId], [DistrictNameAr], [DistrictNameEn], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (NEWID(), source.[DistrictNameAr], NULL, 1, GETUTCDATE(), 0);

GO

-- Verify inserted districts
SELECT COUNT(*) AS TotalDistricts FROM [dbo].[Districts];
GO

PRINT 'Districts seeded successfully!';
GO
