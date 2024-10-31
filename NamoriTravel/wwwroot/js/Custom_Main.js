var GenricFunctions;
var FormatText;
var Alerts;
GenricFunctions = {
    bindDatatable: function (selector, ajaxUrl, columnsConfig, hasDetailsPermission, hasEditPermission, hasDeletePermission) {
        let totalColumns = columnsConfig.length + 1; // Including the "Actions" column
        let columnWidthPercentage = (65 / totalColumns).toFixed(2) + "%";

        if ($.fn.DataTable.isDataTable(selector)) {
            $(selector).DataTable().destroy();
            $(selector).empty();
        }

        var thead = '<thead><tr>';
        columnsConfig.forEach(function (col) {
            thead += '<th class="lang" Key="' + (col.title || col.data) + '" style="width:' + columnWidthPercentage + ';">' + (col.title || col.data) + '</th>';
        });
        thead += '<th class="text-center Key" style="width:' + columnWidthPercentage + ';" Key="Actions">Actions</th></tr></thead>';

        $(selector).prepend(thead);

        return $(selector).DataTable({
            "sAjaxSource": location.origin + ajaxUrl,
            "bServerSide": true,
            "bProcessing": true,
            "autoWidth": false, // We manually control the widths
            "ordering": true,
            "paging": true,
            "scrollX": true, // Enable horizontal scrolling
            "scrollY": "500px", // Limit the table's height for vertical scrolling
            //"autoWidth": true, // Enable auto width adjustment
            "fixedHeader": true, // Enable fixed header
            "language": {
                "emptyTable": "No record found.",
                "processing": '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span>'
            },
            "columns": columnsConfig.map(col => {
                return {
                    "data": col.data,
                    "name": col.name || col.data,
                    "width": columnWidthPercentage, // Apply the calculated width
                    "searchable": col.searchable !== undefined ? col.searchable : false,
                    "orderable": col.orderable !== undefined ? col.orderable : true,
                    "render": function (data, type, row) {
                        if (type === 'display' && typeof data === 'string' && data.length > 20) {
                            return `<span title="${data}">${data.substr(0, 20)}...</span>`;
                        }
                        return data;
                    }
                };
            }),
            "drawCallback": function () {
                var api = this.api(),
                    rowCount = api.rows({ page: 'current' }).count(),
                    emptyRow;

                for (var i = 0; i < api.page.len() - (rowCount === 0 ? 1 : rowCount); i++) {
                    emptyRow = '<tr>';
                    emptyRow += '<td>&nbsp;</td>';
                    for (var columnCounter = 1; columnCounter < totalColumns; columnCounter++) {
                        emptyRow += '<td></td>';
                    }
                    emptyRow += '</tr>';
                    $(selector + ' tbody').append($(emptyRow));
                }
                $("#cover-spin").hide();
                $('select[name="DataTables_Table_0_length"]').addClass('form-select');
            },
            "columnDefs": [
                {
                    "targets": totalColumns - 1, // Assuming the action buttons are in the last column
                    "orderable": false,
                    "render": function (data, type, row) {
                        var html = '<div class="text-center d-flex justify-content-center btnCLICK">';
                        if (hasDetailsPermission) {
                            html += '<button class="btn btn-info view-btn me-2 lang" style="margin-right: 4px;" Key="View" data-id="' + row.id + '">View</button>';
                        }
                        if (hasEditPermission) {
                            html += '<button class="btn btn-warning edit-btn me-2 lang" style="margin-right: 4px;"  Key="Edit" data-id="' + row.id + '">Edit</button>';
                        }
                        if (hasDeletePermission) {
                            html += '<button class="btn btn-danger delete-btn lang" style="margin-right: 4px;" Key="Delete"  data-id="' + row.id + '">Delete</button>';
                        }
                        html += '</div>';
                        return html;
                    }
                }
            ]
        });
    },
    loadRecordData: function (mode, id, Url) {
        globalId = id;
        $.ajax({
            type: 'GET',
            url: location.origin + Url,
            data: { "id": id },
            beforeSend: function () {
                // Show loading indicator
            },
            success: function (response) {
                $('#modalFields').empty();
                let Data = response.data;
                let allData = response.allData;
                let role = response.role;
                let department = response.department;
                let group = response.group;


                for (var key in Data) {
                    if (Data.hasOwnProperty(key)) {
                        let fieldHtml = GenricFunctions.generateFormField(key, Data[key], allData,role,department,group);
                        $('#modalFields').append(fieldHtml);
                    }
                }

                FormatText.ToggleModalMode(mode);
                $('#recordModal').modal('show');
            },
            error: function (error) {
                console.log("Error: ", error);
            },
            complete: function () {
                // Hide loading indicator
            }
        });

    },
    generateFormField: function (key, value, AllData, role, department, group) {

        let inputType = 'text'; // Default input type
        // Define input types based on the key
        if (key.toLowerCase().includes('email')) {
            inputType = 'email';
        } else if (key.toLowerCase().includes('password')) {
            inputType = 'password';
        } else if (key.toLowerCase().includes('date')) {
            inputType = 'date';
        } else if (key.toLowerCase().includes('number') || key.toLowerCase().includes('id') || key.toLowerCase().includes('position') || ['id', 'createdby', 'modifiedby', 'updateby'].includes(key.toLowerCase())) {
            inputType = 'number';
        }
        //------------------User Page Dropdowns  Role,Group and Department defined based on Key--------------------//
        if (key === "roleId") {
            let dropdown = `<select class="form-select lang" Key="${key}" id="ddl${key}" name="${key}">`;
            if (value === 0) {
                dropdown += `<option value="0" selected class="lang" Key="ddlSelect">--Select--</option>`;
            } else {
                dropdown += `<option value="0"  class="lang" Key="ddlSelect">--Select--</option>`;
            }
            role.forEach(rowData => {
                let selected = rowData.id === value ? 'selected' : '';
                dropdown += `<option value="${rowData.id}" ${selected} class="lang" Key="${rowData.roleName}">${rowData.roleName}</option>`;
            });
            dropdown += `</select>`;
            var Lable = FormatText.LabelText(key);
            return `
            <div class="col-md-6 mb-3">
                <label for="${key}" class="form-label lang" Key="${Lable}">${Lable}</label>
                ${dropdown}
            </div>
        `;
        }

        if (key === "groupId") {
            let dropdown = `<select class="form-select lang" Key="${key}" id="ddl${key}" name="${key}">`;
            if (value === 0) {
                dropdown += `<option value="0" selected class="lang" Key="ddlSelect">--Select--</option>`;
            } else {
                dropdown += `<option value="0"  class="lang" Key="ddlSelect">--Select--</option>`;
            }
            group.forEach(rowData => {
                let selected = rowData.id === value ? 'selected' : '';
                dropdown += `<option value="${rowData.id}" ${selected} class="lang" Key="${rowData.groupName}">${rowData.groupName}</option>`;
            });
            dropdown += `</select>`;
            var Lable = FormatText.LabelText(key);
            return `
            <div class="col-md-6 mb-3">
                <label for="${key}" class="form-label lang" Key="${Lable}">${Lable}</label>
                ${dropdown}
            </div>
        `;
        }

        if (key === "departmentId") {
            let dropdown = `<select class="form-select lang" Key="${key}" id="ddl${key}" name="${key}">`;
            if (value === 0) {
                dropdown += `<option value="0" selected class="lang" Key="ddlSelect">--Select--</option>`;
            } else {
                dropdown += `<option value="0"  class="lang" Key="ddlSelect">--Select--</option>`;
            }
            department.forEach(rowData => {
                let selected = rowData.id === value ? 'selected' : '';
                dropdown += `<option value="${rowData.id}" ${selected} class="lang" Key="${rowData.departmentName}">${rowData.departmentName}</option>`;
            });
            dropdown += `</select>`;
            var Lable = FormatText.LabelText(key);
            return `
            <div class="col-md-6 mb-3">
                <label for="${key}" class="form-label lang" Key="${Lable}">${Lable}</label>
                ${dropdown}
            </div>
        `;
        }
        //-----------Parent Page ID Dropdown for Page defined based on Key---------------//
        if (key === "parentPageId") {
            let dropdown = `<select class="form-select lang" Key="${key}" id="ddl${key}" name="${key}">`;
            if (value === 0) {
                dropdown += `<option value="0" selected class="lang" Key="ddlSelect">--Select--</option>`;
            } else {
                dropdown += `<option value="0"  class="lang" Key="ddlSelect">--Select--</option>`;
            }
            AllData.forEach(rowData => {
                let selected = rowData.id === value ? 'selected' : '';
                dropdown += `<option value="${rowData.id}" ${selected} class="lang" Key="${rowData.pageName}">${rowData.pageName}</option>`;
            });
            dropdown += `</select>`;
            var Lable = FormatText.LabelText(key);
            return `
            <div class="col-md-6 mb-3">
                <label for="${key}" class="form-label lang" Key="${Lable}">${Lable}</label>
                ${dropdown}
            </div>
        `;
        } else {
            // put all fields name here in lower case to hidden on View-Edit-Add Modal based on Key
            let isHiddenField = ['id','currency', 'createdby', 'createddate', 'modifieddate', 'modifiedby', 'isactive', 'isdeleted', 'ugroupname', 'udepartmentname', 'urolename', 'attr_css1', 'attr_css2', 'attr_css3', 'attr_css4'].includes(key.toLowerCase());
            let fieldLabel = FormatText.LabelText(key);
            // Check if the key contains the word "date" and format the value if needed
            if (key.toLowerCase().includes('date') && value) {
                console.log("Check Date value: " + value);
                if (value != null || value != 'undefined')
                    value = new Date(value).toISOString().split('T')[0];
            }
            return `
            <div class="col-md-6 mb-3" ${isHiddenField ? 'style="display: none;"' : ''}>
                <label for="${key}" class="form-label lang" Key="${fieldLabel}">${fieldLabel}</label>
                <input type="${inputType}" class="form-control lang" Key="${fieldLabel}" placeholder="" id="${key}" name="${key}" value="${value || ''}">
            </div>
        `;
            
        }
    },
    confirmDelete: function (url, id, table) {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    url: location.origin + url,
                    data: { "id": id },
                    success: function (response) {
                        Swal.fire(
                            'Deleted!',
                            'Your record has been deleted.',
                            'success'
                        ).then(() => {
                            // Reload your DataTable or refresh the page here if necessary
                            table.ajax.reload(); // Example of reloading a DataTable
                        });
                    },
                    error: function (error) {
                        Swal.fire(
                            'Error!',
                            'There was a problem deleting your record.',
                            'error'
                        );
                    }
                });
            }
        });
    }

}

// Replace underscores with spaces and add a space before capital letters
FormatText = {
    LabelText: function (labelName) {
        return labelName
            .replace(/_/g, ' ')                 // Replace underscores with spaces
            .replace(/([a-z])([A-Z])/g, '$1 $2') // Add space before capital letters
            .replace(/([a-zA-Z])(\d+)/g, '$1 $2') // Add space before numbers
            .replace(/\b(\w)/g, function (match) { // Capitalize the first letter of each word
                return match.toUpperCase();
            });
    },
    ToggleModalMode: function (mode) {
        console.log("Mode: " + mode);

        // Hide both save buttons by default
        $('#saveChanges').hide();
        $('#saveNew').hide();

        if (mode === 'edit') {
            $('#recordModalLabel').text("Edit Details");
            $('#saveChanges').show(); // Show the save button for edit mode
            $('#recordForm input, #recordForm textarea, #recordForm select').prop('disabled', false);
        } else if (mode === 'add') {
            $('#recordModalLabel').text("Add Details");
            $('#saveNew').show(); // Show the save button for add mode
            $('#recordForm input, #recordForm textarea, #recordForm select').prop('disabled', false);
        } else {
            $('#recordModalLabel').text("View Details");
            $('#recordForm input, #recordForm textarea, #recordForm select').prop('disabled', true);
        }
    }



}

//-------------- Alerts-----------------//
Alerts = {
    showSuccessMessage: function (action) {
        var message = '';

        switch (action) {
            case 'add':
                message = 'Record added successfully!';
                break;
            case 'update':
                message = 'Record updated successfully!';
                break;
            case 'delete':
                message = 'Record deleted successfully!';
                break;
        }

        Alerts.displayAlert('success', message);
    },

    showErrorMessage: function (action) {
        var message = '';

        switch (action) {
            case 'add':
                message = 'Error adding record.';
                break;
            case 'update':
                message = 'Error updating record.';
                break;
            case 'delete':
                message = 'Error deleting record.';
                break;
        }

        Alerts.displayAlert('danger', message);
    },

    displayAlert: function (type, message) {
        var alertHTML = `<div class="alert alert-${type} alert-dismissible fade show" role="alert">
                        ${message}
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>`;
        $('#notification-container').append(alertHTML);

        // Automatically remove the alert after 3 seconds
        setTimeout(function () {
            $('.alert').alert('close');
        }, 3000);
    }

}
