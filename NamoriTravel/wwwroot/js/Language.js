function applyTranslations() {
    const translations = {
        "Full Name": "Full Name / الاسم الكامل",
        "EmiratesID": "Emirates ID / رقم الهوية الإماراتية",
        "DocumentIDNumber": "Document ID/Number / رقم المرجع",
        "DocumentType": "Document Type / نوع المستند",
        "Gender": "Gender / الجنس",
        "Male": "Male / ذكر",
        "Female": "Female / أنثى",
        "MobileNumber": "Mobile Number / رقم الهاتف المتحرك",
        "VisitorType": "Visitor Type / نوع الزائر",
        "SelectType": "Select Type / اختر النوع",
        "Passport": "Passport / جواز السفر",
        "DrivingLicense": "Driving License / رخصة القيادة",
        "GroundFloor": "Ground Floor / الطابق الأرضي",
        "FirstFloor": "First Floor / الطابق الأول",
        "SecondFloor": "Second Floor / الطابق الثاني",
        "Email": "Email / البريد الإلكتروني",
        "ServiceType": "Service Type / نوع الخدمة",
        "CompanyFrom": "Company From / نموذج الشركة",
        "TokenNumber": "Token Number / رقم التذكرة",
        "License": "License / الرخصة",
        "Interaction": "Interaction / التفاعل",
        "OtherInformation": "Other Information / معلومات أخرى",
        "PurposeOfVisit": "Purpose of Visit / سبب الزيارة",
        "WhomToVisit": "Whom to Visit / لمن الزيارة",
        "WhomToMeet": "Whom To Meet / لمن نلتقي",
        "DepartmentToVisit": "Department to Visit / الزيارة الى إدارة",
        "ExpectedCheckOut": "Expected Check Out / الوقت المتوقع للخروج",
        "EntryTime": "Entry time / وقت الدخول",
        "Nationality": "Nationality / الجنسية",
        "Notes": "Notes / ملاحظات",
        "Address": "Address / العنوان",
        "Picture": "Picture / الصورة",
        "Save": "Save / حفظ",
        "Scan": "Scan / مسح ضوئي",
        "AddVisitors": "Add Visitors / إضافة زوار",
        "FromDate": "From Date / من تاريخ",
        "ToDate": "To Date / إلى تاريخ",
        "Status": "Status / الحالة",
        "Floor": "Floor / الطابق",
        "Search": "Search / البحث",
        "WithCheckout": "With Checkout / مع تسجيل الخروج",
        "WithoutCheckout": "Without Checkout / بدون تسجيل خروج",
        "Company": "Company / الشركة",
        "CheckIn": "Check In / الدخول",
        "CheckOut": "Check Out / الخروج",
        "Detail": "Detail / التفاصيل",
        "Filter": "Filter / التصفية",
        "Name": "Name / الاسم",
        "AddUser": "Add User / إضافة مستخدم",
        "UserID": "User ID / هويةالمستخدم",
        "SelectBranch": "Select Branch / اختيار الفرع",
        "Group": "Group / المجموعة",
        "SelectGroup": "Select Group / اختر المجموعة",
        "SelectCompany": "Select Company / اختر الشركة",
        "UserName": "User Name / اسم المستخدم",
        "UserEmail": "User Email / البريد الإلكتروني للمستخدم",
        "UserCompany": "User Company / اسم المستخدم للشركة",
        "UserDepartment": "User Department / اسم المستخدم للإدارة",
        "UserBranch": "User Branch / اسم المستخدم للفرع",
        "Modification": "Modification / التعديل",
        "Edit": "Edit / التعديل",
        "Delete": "Delete / حذف",
        "AddEmployee": "Add Employee / إضافة موظف",
        "EmployeeID": "Employee ID / هوية الموظف",
        "Designation": "Designation / التعيين",
        "Status": "Status / الحالة",
        "ViewDetail": "View Detail / عرض التفاصيل",
        "Disable": "Disable / إيقاف",
        "Enable": "Enable / ممكن",
        "View": "View / عرض",
        "AddDepartments": "Add Departments / إضافة الإدارة",
        "Departments": "Departments / الإدارات",
        "Department": "Department / قسم",
        "CompanyName": "Company Name / اسم الشركة",
        "Close": "Close / إغلاق",
        "EditUser": "Edit user / تعديل المستخدم",
        "Branch": "Branch / الفرع",
        "CompanyAdmin": "Company Admin / المسؤول الإداري",
        "AddVisitor": "Add Visitor / إضافة زائر",
        "ViewVisitor": "View Visitor / عرض الزوار",
        "Admin": "Admin / المسؤول",
        "Appointment": "Appointment / الموعد",
        "AddAppointment": "Add Appointment / إضافة الموعد",
        "AppointmentTimeStart": "Appointment Time Start / يبدا الموعد",
        "AppointmentDate": "Appointment Date / تاريخ الموعد",
        "AppointmentTimeEnd": "Appointment Time End / ينتهي الموعد",
        "CardNumber": "Card Number / رقم البطاقة",
        "PassportNumber": "Passport Number / رقم الجواز السفر",
        "ViewGroupList": "View Group List / عرض قائمة المجموعة",
        "RolesAndPermission": "Roles & Permission / السياسات و الصلاحيات",
        "AddGroup": "Add Group / إضافة مجموعة",
        "GroupName": "Group Name / اسم المجموعة",
        "AssignPermission": "Assign Permission / توجيه صلاحية",
        "PageName": "Page Name / اسم الصفحة",
        "Operation": "Operation / العملية",
        "Permission": "Permission / الصلاحية",
        "Employee": "Employee / الموظف",
        "PageView": "PageView / عرض الصفحة",
        "VisitorDetail": "Visitor Detail / تفاصيل الزائر",
        "Comments": "Comments / تعليق",
        "Password": "Password / كلمه السر ",
        "SelectFloor": "Select Floor / حدد الطوابق",
        "IsEnable": "Is Enable / هو تمكين",
        "AppointmentTime": "Appointment Time / وقت الموعد",
        "Branches": "Branches / الفروع",
        "Update": "Update / تحديث",
        "Cancel": "Cancel / إلغاء",
        "Mobile": "Mobile / هاتف محمول",
        "EmiratesNumber": "Emirates Number"
        };

        document.querySelectorAll('.lang').forEach(function (element) {
            const key = element.getAttribute('Key');
            if (translations[key]) {
                if (element.tagName === "INPUT" || element.tagName === "TEXTAREA") {
                    element.setAttribute("placeholder", translations[key]);
                } else if (element.tagName === "OPTION") {
                    element.text = translations[key];
                } else if (element.tagName === "TH") {  // For DataTable headers
                    element.innerHTML = translations[key];
                } else if (element.tagName === "LABEL") {  // For labels associated with radio buttons, checkboxes, etc.
                    element.innerHTML = translations[key];
                } else if (element.tagName === "SELECT") {  // For dropdowns
                    element.querySelectorAll('option').forEach(function (option) {
                        const optionKey = option.getAttribute('Key');
                        if (translations[optionKey]) {
                            option.text = translations[optionKey];
                        }
                    });
                } else if (element.tagName === "INPUT" && (element.type === "radio" || element.type === "checkbox")) {  // For radio buttons and checkboxes
                    let label = document.querySelector(`label[for="${element.id}"]`);
                    if (label && translations[key]) {
                        label.innerHTML = translations[key];
                    }
                } else {
                    element.innerHTML = translations[key];
                }
            }
        });
}
