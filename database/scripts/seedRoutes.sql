-- =============================================
-- TUMS Routes Seed (MEN Routes)
-- Target: [TumsDb].[dbo].[Routes]
-- Safe to run multiple times (idempotent)
-- =============================================

SET NOCOUNT ON;
BEGIN TRANSACTION;

BEGIN TRY
    DECLARE @Routes TABLE (RouteName NVARCHAR(500));

    INSERT INTO @Routes (RouteName) VALUES
    (N'العزيزية مسلم'),
    (N'م . باقدو/ م . فهد'),
    (N'الكردي / السيح'),
    (N'الدعيثة ابو مرخة'),
    (N'الجرف الغربي'),
    (N'الصالحية'),
    (N'العنابس/ القبلتين / الغربية'),
    (N'الغربية / العنبرية / السقيا'),
    (N'المصانع / المستراح'),
    (N'المستراح/السحمان'),
    (N'الروضة/الكويتية'),
    (N'العوالي'),
    (N'قربان / الخاتم / البحر'),
    (N'الأعمدة / شارع فلسطين'),
    (N'عروة /الوبرة'),
    (N'الرانوناء / شوران'),
    (N'الجرف الشرقى'),
    (N'العزيزية البخارى'),
    (N'م طيبة / الدويمة/الظاهرة/العصبة'),
    (N'العوالي/السديري'),
    (N'المغاربة /البحر/قربان'),
    (N'شوران / الروابي'),
    (N'مخطط الزهرة'),
    (N'مخطط الأمير نايف / القصواء'),
    (N'النصر'),
    (N'السديري / الحزام'),
    (N'الخليل'),
    (N'الشهداء / الطيارية'),
    (N'الفيصلية/واحة السلام'),
    (N'الحرس / الشافية'),
    (N'النسيم / الأزهري'),
    (N'التلال / الشيبية'),
    (N'الربوة'),
    (N'الخالدية'),
    (N'الخالدية/الاسكان الشمالي'),
    (N'الحزام/المبعوث'),
    (N'مخطط الأمير تركي المطار'),
    (N'المغيسلة/الجبور/ الزاهدية'),
    (N'الملك فهد'),
    (N'مهزور/الإسكان الجنوبي/الروابي'),
    (N'سلطانة');

    INSERT INTO [TumsDb].[dbo].[Routes]
    (
        RouteId,
        RouteName,
        RouteDescription,
        IsActive,
        CreatedAt,
        CreatedBy,
        IsDeleted
    )
    SELECT
        NEWID(),
        r.RouteName,
        r.RouteName,
        1,
        GETUTCDATE(),
        NULL,
        0
    FROM @Routes r
    WHERE NOT EXISTS (
        SELECT 1
        FROM [TumsDb].[dbo].[Routes] db
        WHERE db.RouteName = r.RouteName
          AND db.IsDeleted = 0
    );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;
