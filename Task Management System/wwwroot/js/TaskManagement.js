
var taskManagement = (function () {

    var searchTasks = () =>{
        var status = $("#status").val(); // Get the selected status value

        // AJAX request to update the table
        $.ajax({
            url: '/TaskManagement/GetAllTask', // Update the URL with your search action
            type: 'GET',
            data: { status: status },
            success: function (result) {
                // Replace the table content with the updated content
                $('tbody').empty(); // Remove all existing rows
                $('tbody').html(result);
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    }

    var addTask = (url, title) => {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (res) {
                $('#form-modal .modal-body').html(res);
                $('#form-modal .modal-title').html(title);
                $('#form-modal').modal('show');
                $('#form-modal').on('shown.bs.modal', function () {
                    $('#Title').focus();
                })
                $('#form-modal').css('z-index', 1050); // You can adjust this value as needed
                $('.modal-backdrop').css('z-index', 1040); // Make sure this value is lower than modal's z-index
            }
        })
    }
    var saveTask = form => {
        let taskManagement = $("#formTaskManagement")[0];
        try {
            $.ajax({
                url: '/TaskManagement/Save',
                type: 'POST',
                data: new FormData(taskManagement),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.isValid) {
                        $('#form-modal .modal-body').html('');
                        $('#form-modal .modal-title').html('');
                        $('#form-modal').modal('hide');
                        ShowSnackBarSuccessMessage(res.message);
                        refreshTableData();
                    }
                    else {
                        /*$('#form-modal .modal-body').html(res.html);*/
                        ShowModelDialogFailureAlert(res.message);
                    }
                },
                error: function (error) {
                    console.error(error);
                }
            });
            // to prevent default form submit event
            return false;
        } catch (ex) {
            console.log(ex);
            ShowModelDialogFailureAlert("Failed to save the data");
        }
        return false;
    }


    var editTask = (editUrl, action) => {
        // Assuming you have jQuery available for AJAX
        $.ajax({
            url: editUrl,
            type: 'GET',
            success: function (res) {
                $('#form-modal .modal-body').html(res);
                $('#form-modal .modal-title').html('Edit');
                $('#form-modal').modal('show');
                $('#form-modal').on('shown.bs.modal', function () {
                    $('#Title').focus();
                })
                $('#form-modal').css('z-index', 1050); // You can adjust this value as needed
                $('.modal-backdrop').css('z-index', 1040); // Make sure this value is lower than modal's z-index
            }
        });
    }

    var deleteTask = (editUrl, action) => {
        if (confirm('Are you sure to delete this record ?')) {
            try {
                jQuery.ajax({
                    type: 'POST',
                    url: editUrl,
                    success: function (res) {
                        if (res.isValid) {
                            ShowSnackBarSuccessMessage(res.message);
                            refreshTableData();
                        }
                        else {
                            ShowSnackBarFailureMessage(res.message);
                        }
                    },
                    error: function (err) {
                        console.log(err)
                        ShowSnackBarFailureMessage("failed to delete AccountType, please try again!")
                    }
                })
            } catch (ex) {
                console.log(ex)
                ShowSnackBarFailureMessage("failed to delete AccountType, please try again!")
            }
        }
    }
    function refreshTableData() {
        setTimeout(function () {
            window.location.reload();
        }, 1000);
    }
    // Function to refresh the table data (assuming server-side data retrieval)
    function refreshTableDatas() {
        // 1. Fetch updated data from server using AJAX
        $.ajax({
            url: '/TaskManagement/GetAll', // Replace with your actual endpoint
            success: function (data) {
                // 2. Clear existing table content
                $('tbody').empty(); // Remove all existing rows

                // 3. Loop through the new data and populate the table
                $.each(data, function (index, task) {
                    var formattedDueDate = new Date(task.dueDate).toLocaleDateString('en-US', { year: 'numeric', month: '2-digit', day: '2-digit' });

                    var newRow = `<tr>
          <td>${task.title}</td>
          <td>${task.description}</td>
          <td>${formattedDueDate}</td>
          <td>${task.taskStatus}</td>
          <td style="width:50px;">
            <a href="javascript:void(0);" onclick="taskManagement.editTask('@Url.Action("Edit", "TaskManagement", new { id = task.Id })', 'Edit')">Edit</a> |
            <a href="javascript:void(0);" onclick="taskManagement.deleteTask('@Url.Action("Delete", "TaskManagement", new { id = task.Id })')">Delete</a>
          </td>
        </tr>`;
                    $('tbody').append(newRow);
                });
            },
            error: function (err) {
                console.log(err);
                ShowSnackBarFailureMessage("Failed to refresh task list!");
            }
        });
    }

    return {
        searchTasks,
        addTask,
        saveTask,
        editTask,
        deleteTask
    }

})();