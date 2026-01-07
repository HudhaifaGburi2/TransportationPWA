SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER VIEW [dbo].[vw_Student_Halaqa_Teacher_information_Transportation_Dep]
AS
SELECT        
    -- Student Information
	s.User_ID AS student_user_id,-- user id match regt_user table and student table
    s.Student_ID AS student_id ,--like S154352,
    COALESCE(ru_student.Full_Name_Official_AR, '') AS student_name,
    ru_student.User_Language AS student_language,

    -- Education Type Information (Arabic)
    et.Education_Type_ID AS education_type_id,
    et.Edu_Type_Desc AS education_type_name,
    et.Edu_Type_Code AS education_type_code,

    -- Halaqa Type Information (Arabic)
    ht.Halaqa_ID AS halaqa_type_id,
    ht.Halaqa_Desc AS halaqa_type_name,
    ht.Halaqa_Code AS halaqa_type_code,

    -- Memorization Level Information (Arabic)
    ml.Memorization_Level_ID AS memorization_level_id,
    ml.Memorization_Level_Desc AS memorization_level_name,

    -- Teacher Information
    t.T_DB_ID AS teacher_db_id,
    t.Teacher_ID AS teacher_id,
    COALESCE(ru_teacher.Full_Name_Official_AR, '') AS teacher_name,
    ru_teacher.User_Id AS teacher_user_id,

    -- Period Information (Arabic)
    p.Period_ID AS period_id,
    p.Period_Desc AS period_name,
    p.Period_Code AS period_code,
    p.StartTime AS period_start_time,
    p.EndTime AS period_end_time,

    -- Age Group Information (Arabic)
    ag.Age_Group_ID AS age_group_id,
    ag.AG_Name AS age_group_name,
    ag.Min_Age_Limit,
    ag.Max_Age_Limit,

    -- Halaqa Location Information
    hl.Location_ID AS halaqa_location_id,
    hl.Name AS halaqa_location_name,
    hl.Gender AS halaqa_gender,

    -- MAP_Std_Edu_period Information
    sep.MAP_Student_ID AS map_std_edu_period_id,
    sep.Status_SEP_KeyNUM AS map_status,

    -- Halaqa Section Information
    hs.Halaqa_Sec_ID AS halaqa_section_id,
    hs.Halaqa_Status_KeyNUM_49 AS halaqa_section_status,

    -- Teacher–Halaqa Mapping
    hs.Halaqa_Sec_ID AS teacher_halaqa_sec_id,

    -- Student–Halaqa Mapping
    mhss.Halaqa_Sec_ID AS student_halaqa_sec_id

FROM dbo.Student AS s
    INNER JOIN dbo.Regt_user AS ru_student
        ON s.User_ID = ru_student.User_Id

    INNER JOIN dbo.MAP_HALAQA_Sec_Std AS mhss
        ON mhss.Student_ID = s.Student_ID

    INNER JOIN dbo.HALAQAT_Section AS hs
        ON mhss.Halaqa_Sec_ID = hs.Halaqa_Sec_ID

    LEFT JOIN dbo.SET_Education_Type AS et
        ON hs.Education_Type_ID = et.Education_Type_ID
       AND et.Language_ET = 'ar-SA'

    LEFT JOIN dbo.SET_Age_Group AS ag
        ON hs.Age_Group_ID = ag.Age_Group_ID
       AND ag.AG_Language = 'ar-SA'

    LEFT JOIN dbo.SET_Halaqat AS ht
        ON hs.Halaqa_ID = ht.Halaqa_ID
       AND ht.Language_H = 'ar-SA'

    LEFT JOIN dbo.SET_Period AS p
        ON hs.Period_ID = p.Period_ID
       AND p.Language_P = 'ar-SA'

    LEFT JOIN dbo.MAP_Std_Edu_period AS sep
        ON sep.Student_ID = s.Student_ID
       AND sep.Education_Type_ID = hs.Education_Type_ID
       AND sep.Period_ID = hs.Period_ID
       AND sep.Age_Group_ID = hs.Age_Group_ID

    LEFT JOIN dbo.SET_Memorization_Level AS ml
        ON sep.Memorization_Level_ID = ml.Memorization_Level_ID
       AND ml.Language_M = 'ar-SA'

    LEFT JOIN dbo.Teacher AS t
        ON hs.Teacher_ID = t.Teacher_ID

    LEFT JOIN dbo.Regt_user AS ru_teacher
        ON t.User_ID = ru_teacher.User_Id

    LEFT JOIN dbo.HALAQAT_Location AS hl
        ON sep.Halaqa_Location_ID = hl.Location_ID;
GO