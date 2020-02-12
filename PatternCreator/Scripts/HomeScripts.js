'use strict';


$(document).ready(function() {


 

    $(".CompanyEdit").click(function(e) {

        let span = $(e.target).parents()[2].children[0].children[0];
        let EditorContain = $(e.target).parents()[2].children[0].children[1];
        let button = $(e.target).parents()[2].children[0].children[2];


        $(button).css("display", "none");
        $(EditorContain).css("display", "flex");
        $(span).css("display", "none");



    });


    $(".CompanyAdd").click(function() {

        let block = $(`<div id="TableTwo">
          <table id="TableTwoStyle">

              <tr>
                  <div class="CompanyAligment">

                      <div class="SaveAligment" style="width: 230px;">
                      <input type="text" class="EditCompanyInput">

                    
                       <!-- <img src="/Главная страница/svg/save-icon-silhouette.svg" class="SaveIconStyle"> -->
          
                      </div>
                      <div class="IconUtilitiesCompany">
                          <button class="CompanyAdd">
                              <img src="/Resourses/svg/add.svg" class="ManageIconStyleCompany">
                          </button>

                          <button class="CompanyEdit">
                              <img src="/Resourses/svg/pencil-edit-button (2).svg" class="ManageIconStyleCompany">
                          </button>

                          <button class="CompanyDelete">
                              <img src="/Resourses/svg/delete.svg" class="ManageIconStyleCompany">
                          </button>
                          
                          <button class='IconButtonsSave'>
                              <img src='/Resourses/svg/save-file.svg' class='ManageIconStyle'>
                          </button>
                          
                         
                      </div>
                     
                    
                   </div>
              </tr>
              </table>
              </div>`);


        console.log($("table:last"))
        $("table:last").after(block);


    });

    //$(".FindButton:button").click(function() {
    //    let inputText = $(".ControlText:input").val();

    //    let badTd = $("tr>tr");


    //    let tables = $("tr > td > span");


    //    for (var i = 0; i < tables.length; i++) {
    //        if (inputText != $(tables[i]).text()) {
    //            $(tables[i]).css("display", "none");


    //        }



    //    }




    //    console.log(inputText);


    //});



});


