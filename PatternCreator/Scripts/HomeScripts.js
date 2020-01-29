'use strict';


$(document).ready(function(){


    $("tr.normalTr").hover(function () {
        $(this).addClass("ColoredTr")
        let block = $(`<div class='IconUtilities'>
              
        

        <button class='IconButtonsEdit'>
            <img src='/Главная страница/svg/pencil-edit-button (2).svg' class='ManageIconStyle'>
        </button>

        <button class='IconButtonsDelete'>
            <img src='/Главная страница/svg/delete.svg' class='ManageIconStyle'>
        </button>

        <button class='IconButtonsSave'>
            <img src='/Главная страница/svg/save-file.svg' class='ManageIconStyle'>
        </button>



          </div>`);

        
        $(this).append(block)

        $(".IconButtonsEdit").click(function(){
          
          
          let input = $(".ColoredTr > td");
          
          // $(input).makeArray();

          let bufer = $.makeArray(input);
          console.log(input);
            

          let block = $(`<td><input type="text" class="EditCol"></td>`);
          $(input).replaceWith(block);

            
            
         

        });

        $(".IconButtonsSave").click(function(){
            let input = $(".EditCol:input")
            console.log(input);
            let block = $(`<span>Какая то информация</span>`)

            input.replaceWith(block);
        })


        $(".IconButtonsDelete").click(function(){

              console.log($(".ColoredTr"));

              $(".ColoredTr").remove();
        })

        // $(".IconButtonsAdd").click(function(){

        //     let block = $(`<tr class="">
        //     <td><span>1</span></td>
        //     <td class="EditTdCol">
        //         <div class="EditCol">
        //             <span class="EditTextCol">Имя</span> </div>
        //     </td>
        //     <td class="EditTdCol">
        //         <div class="EditCol">
        //             <span class="EditTextCol">Фамилия</span>
        //         </div>
        //     </td>
        //     <td class="EditTdCol">
        //         <div class="EditCol">
        //             <span class="EditTextCol">Отчество</span>
        //         </div>
        //     </td>
        // </tr>`)

        //      console.log($("table tr:nth-last-of-type(1)"));
        //      $("table tr:nth-last-of-type(1)").after(block);


        // })


        
        



        
      }, function () {
       
            $(".ColoredTr").removeClass();
              $("div.IconUtilities").remove();

      }
    );

     
    $(".CompanyEdit").click(function(){

      let block = $(`<input type="text" class="EditCompanyInput">`)
     
      $("div.SaveAligment > span").replaceWith(block);

      $("button.IconButtonsSave").click(function(){
        

        let blockSpan = $(` <span id="CompanyStyleEdit">ООО "Гефест строй"</span>`)
        
        $(block).replaceWith(blockSpan);

      })

      


    })


    $(".CompanyDelete").click(function(){

      let answer = confirm("Вы уверены?")
      if(answer == true){
         $("table#TableTwoStyle").remove();
      }
       else{
         return false;
       }
    })



      function manageCompanyIcons(){
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
                            <img src="/Главная страница/svg/add.svg" class="ManageIconStyleCompany">
                        </button>

                        <button class="CompanyEdit">
                            <img src="/Главная страница/svg/pencil-edit-button (2).svg" class="ManageIconStyleCompany">
                        </button>

                        <button class="CompanyDelete">
                            <img src="/Главная страница/svg/delete.svg" class="ManageIconStyleCompany">
                        </button>
                        
                        <button class='IconButtonsSave'>
                            <img src='/Главная страница/svg/save-file.svg' class='ManageIconStyle'>
                        </button>
                        
                       
                    </div>
                   
                  
                 </div>
            </tr>
            </table>
            </div>`);


            console.log($("table:last"))
        $("table:last").after(block);
       
      }




    $(".CompanyAdd").click(function(){

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
                              <img src="/Главная страница/svg/add.svg" class="ManageIconStyleCompany">
                          </button>

                          <button class="CompanyEdit">
                              <img src="/Главная страница/svg/pencil-edit-button (2).svg" class="ManageIconStyleCompany">
                          </button>

                          <button class="CompanyDelete">
                              <img src="/Главная страница/svg/delete.svg" class="ManageIconStyleCompany">
                          </button>
                          
                          <button class='IconButtonsSave'>
                              <img src='/Главная страница/svg/save-file.svg' class='ManageIconStyle'>
                          </button>
                          
                         
                      </div>
                     
                    
                   </div>
              </tr>
              </table>
              </div>`);


              console.log($("table:last"))
          $("table:last").after(block);

       
    })


    
    
   




});


