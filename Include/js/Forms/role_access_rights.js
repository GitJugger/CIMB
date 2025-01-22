var selectedRows = {};
var table;
$(document).ready(function () {

   


    example();
    test();

    //var label = document.getElementById('#myLabel')
    //label.innerText = "New Value Assigned!";

    //$("#myLabel").text("Test");
    //var x = "";

});
function example() {

    var savedSelected;

    table = new DataTable('#dgUsers', {

        pageLength: 10,
        //lengthMenu: [10, 15, 50, -1],
        searching: true,
        paging: true,
        lengthChange: false,
        "bSort": false
        
        //info: false,
        //ordering: false,
        //responsive: true
    });


    $('#submitBtn').click(function () {
        // Collect table data, including checkbox state
        var tableData = [];

        var allRows = table.rows().nodes(); 

        $(allRows).each(function () {
            
            var menu = $(this).find('td:eq(0)').text().trim(); // ID column
            var page = $(this).find('td:eq(1)').text().trim(); // ItemName column
            var isChecked = $(this).find('input[type="checkbox"]').is(':checked'); // Checkbox column
            var id = $(this).find('td:eq(3)').text().trim(); // ID column

            //console.log(menu);
            //console.log(page);
            //console.log(isChecked);
            //console.log(id);

            // Push data into the array
            tableData.push({
                RightsId: id,
                MenuName: menu,
                PageName: page,
                Rights: isChecked,
            });
        });

        // Submit data via AJAX
        $.ajax({
            type: "POST",
            url: "PG_RoleAccessRights.aspx/SaveData", // Server-side method
            data: JSON.stringify({ tableData: tableData }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                //alert('Data submitted successfully!');
                //$('#<%= lblMessage.ClientID %>').text('Success: ' + response.d);

               
                $("#myLabel").text(response.d)

               // window.location.href = "PG_RoleAccessRights.aspx";
            },
            error: function (error) {
                console.error(error);
                $("#myLabel").text(response.d)
            }
        });



    });

    var table1 = new DataTable('#dgUsersBind', {

        pageLength: 10,
        searching: false,
        paging: true,
        info: true,
        ordering: false,
        responsive: true,

    });
}

function test() {

    
}


