  function filterOrders(orderStateId) {

            var orderStateId = Number(orderStateId);
            $.ajax({
                url: `/Order/GetFilteredOrders?orderState=${orderStateId}`,
                error:function(){
                    console.log("Error");
                },
                success: function (result) {
                    document.getElementById("tableData").innerHTML = "";
                        
                    for (let i of result) {
                        document.getElementById("tableData").innerHTML +=
                            `<tr>
                                <td>
                                    ${i.id}
                                </td>
                                <td>
                                    ${i.creationDate}
                                </td>
                             
                                <td>
                                               ${i.clientName} </br>
                                               ${i.clientPhone1}

                            
                                </td>


                                        <td>
                                                 ${i.clientGovernorateId}

                                    </td>

                                <td>
                                           ${i.clientCityId}
                                 </td>

                                             <td>
                                                    ${i.shippingPrice}
                                         </td>

                    <td>
                                    <a ${asp-action="Edit"} asp-route-id="${i.id}"><i role="button" class="fa-solid fa-pen-to-square text-success"></i></a>
                    </td>
                    <td>
                        <button class="btn btn-primary">Status</button>
                    </td>

                        <td>
                            <form ${asp-action="Delete"}  ${asp-route-id="${i.id}"} method="post" onsubmit="return confirm('Are you sure you want to delete this order?');">
                                        <input type="hidden" name="id" value="${i.id}" />
                                <i role="button" class="fa-solid fa-trash text-danger"></i>
                            </form>
                        </td>

                    <td>
                            <i role="button" class="fa-solid fa-print text-primary"></i>
                        </td>


                </tr>
        `
                 } 
                 }
                }
            });

         
            }