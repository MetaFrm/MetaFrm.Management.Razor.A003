using MetaFrm.Database;
using MetaFrm.Extensions;
using MetaFrm.Management.Razor.Models;
using MetaFrm.Management.Razor.ViewModels;
using MetaFrm.Razor.Essentials;
using MetaFrm.Service;
using MetaFrm.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MetaFrm.Management.Razor
{
    /// <summary>
    /// A003
    /// </summary>
    public partial class A003
    {
        #region Variable
        internal A003ViewModel A003ViewModel { get; set; } = Factory.CreateViewModel<A003ViewModel>();

        internal DataGridControl<CommonClassModel>? DataGridControl;

        internal CommonClassModel SelectItem = new();

        internal CardWindowStatus CardWindowStatusText = CardWindowStatus.Maximize;
        internal CardWindowStatus CardWindowStatusInt = CardWindowStatus.Maximize;
        internal CardWindowStatus CardWindowStatusNumber = CardWindowStatus.Maximize;
        internal CardWindowStatus CardWindowStatusDatetime = CardWindowStatus.Maximize;
        #endregion


        #region Init
        /// <summary>
        /// OnAfterRenderAsync
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!this.IsLogin())
                    this.Navigation?.NavigateTo("/", true);

                this.A003ViewModel = await this.GetSession<A003ViewModel>(nameof(this.A003ViewModel));

                this.Search();

                this.StateHasChanged();
            }
        }
        #endregion


        #region IO
        private void New()
        {
            this.SelectItem = new();
        }

        private void OnSearch()
        {
            if (this.DataGridControl != null)
                this.DataGridControl.CurrentPageNumber = 1;

            this.Search();
        }
        private void Search()
        {
            Response response;

            try
            {
                if (this.A003ViewModel.IsBusy) return;

                this.A003ViewModel.IsBusy = true;

                ServiceData serviceData = new()
                {
                    Token = this.UserClaim("Token")
                };
                serviceData["1"].CommandText = this.GetAttribute("Search");
                serviceData["1"].AddParameter(nameof(this.A003ViewModel.SearchModel.SEARCH_TEXT), DbType.NVarChar, 4000, this.A003ViewModel.SearchModel.SEARCH_TEXT);
                serviceData["1"].AddParameter("USER_ID", DbType.Int, 3, this.UserClaim("Account.USER_ID").ToInt());
                serviceData["1"].AddParameter("PAGE_NO", DbType.Int, 3, this.DataGridControl != null ? this.DataGridControl.CurrentPageNumber : 1);
                serviceData["1"].AddParameter("PAGE_SIZE", DbType.Int, 3, this.DataGridControl != null && this.DataGridControl.PagingEnabled ? this.DataGridControl.PagingSize : int.MaxValue);

                response = serviceData.ServiceRequest(serviceData);

                if (response.Status == Status.OK)
                {
                    this.A003ViewModel.SelectResultModel.Clear();

                    if (response.DataSet != null && response.DataSet.DataTables.Count > 0)
                    {
                        var orderResult = response.DataSet.DataTables[0].DataRows.OrderBy(x => x.String(nameof(CommonClassModel.CLASS_NAME)));

                        foreach (var datarow in orderResult)
                        {
                            this.A003ViewModel.SelectResultModel.Add(new CommonClassModel
                            {
                                CLASS_ID = datarow.Int(nameof(CommonClassModel.CLASS_ID)),
                                CLASS_NAME = datarow.String(nameof(CommonClassModel.CLASS_NAME)),
                                KEY_VALUE = datarow.String(nameof(CommonClassModel.KEY_VALUE)),
                                TEXT_VALUE1 = datarow.String(nameof(CommonClassModel.TEXT_VALUE1)),
                                TEXT_VALUE2 = datarow.String(nameof(CommonClassModel.TEXT_VALUE2)),
                                TEXT_VALUE3 = datarow.String(nameof(CommonClassModel.TEXT_VALUE3)),
                                TEXT_VALUE4 = datarow.String(nameof(CommonClassModel.TEXT_VALUE4)),
                                TEXT_VALUE5 = datarow.String(nameof(CommonClassModel.TEXT_VALUE5)),
                                TEXT_VALUE6 = datarow.String(nameof(CommonClassModel.TEXT_VALUE6)),
                                TEXT_VALUE7 = datarow.String(nameof(CommonClassModel.TEXT_VALUE7)),
                                TEXT_VALUE8 = datarow.String(nameof(CommonClassModel.TEXT_VALUE8)),
                                TEXT_VALUE9 = datarow.String(nameof(CommonClassModel.TEXT_VALUE9)),
                                TEXT_VALUE10 = datarow.String(nameof(CommonClassModel.TEXT_VALUE10)),
                                INT_VALUE1 = datarow.String(nameof(CommonClassModel.INT_VALUE1)),
                                INT_VALUE2 = datarow.String(nameof(CommonClassModel.INT_VALUE2)),
                                INT_VALUE3 = datarow.String(nameof(CommonClassModel.INT_VALUE3)),
                                INT_VALUE4 = datarow.String(nameof(CommonClassModel.INT_VALUE4)),
                                INT_VALUE5 = datarow.String(nameof(CommonClassModel.INT_VALUE5)),
                                NUMBER_VALUE1 = datarow.String(nameof(CommonClassModel.NUMBER_VALUE1)),
                                NUMBER_VALUE2 = datarow.String(nameof(CommonClassModel.NUMBER_VALUE2)),
                                NUMBER_VALUE3 = datarow.String(nameof(CommonClassModel.NUMBER_VALUE3)),
                                NUMBER_VALUE4 = datarow.String(nameof(CommonClassModel.NUMBER_VALUE4)),
                                NUMBER_VALUE5 = datarow.String(nameof(CommonClassModel.NUMBER_VALUE5)),
                                DATETIME_VALUE1 = datarow.String(nameof(CommonClassModel.DATETIME_VALUE1)),
                                DATETIME_VALUE2 = datarow.String(nameof(CommonClassModel.DATETIME_VALUE2)),
                                DATETIME_VALUE3 = datarow.String(nameof(CommonClassModel.DATETIME_VALUE3)),
                                DATETIME_VALUE4 = datarow.String(nameof(CommonClassModel.DATETIME_VALUE4)),
                                DATETIME_VALUE5 = datarow.String(nameof(CommonClassModel.DATETIME_VALUE5)),
                                INACTIVE_DATE = datarow.DateTime(nameof(CommonClassModel.INACTIVE_DATE)),
                            });
                        }

                        //this.DataGridControl?.SortInit(this.ColumnDefinitions, nameof(SelectResultModel.NAMESPACE), SortDirection.Ascending);
                        this.DataGridControl?.SortData();
                        //this.DataGridControl.Pages = new int[] { 1, 2, 3, 4 };
                    }
                }
                else
                {
                    if (response.Message != null)
                    {
                        this.ModalShow("Warning", response.Message, new() { { "Ok", Btn.Warning } }, null);
                    }
                }
            }
            catch (Exception e)
            {
                this.ModalShow("An Exception has occurred.", e.Message, new() { { "Ok", Btn.Danger } }, null);
            }
            finally
            {
                this.A003ViewModel.IsBusy = false;
                this.SetSession(nameof(A003ViewModel), this.A003ViewModel);
            }
        }

        private void Save()
        {
            Response? response;
            string? value;

            response = null;

            try
            {
                if (this.A003ViewModel.IsBusy)
                    return;

                if (this.SelectItem == null)
                    return;

                this.A003ViewModel.IsBusy = true;

                ServiceData serviceData = new()
                {
                    TransactionScope = true,
                    Token = this.UserClaim("Token")
                };
                serviceData["1"].CommandText = this.GetAttribute("Save");
                serviceData["1"].AddParameter(nameof(this.SelectItem.CLASS_ID), DbType.Int, 3, "2", nameof(this.SelectItem.CLASS_ID), this.SelectItem.CLASS_ID);
                serviceData["1"].AddParameter(nameof(this.SelectItem.CLASS_NAME), DbType.NVarChar, 200, this.SelectItem.CLASS_NAME);
                serviceData["1"].AddParameter(nameof(this.SelectItem.KEY_VALUE), DbType.NVarChar, 200, this.SelectItem.KEY_VALUE);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE1), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE1);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE2), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE2);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE3), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE3);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE4), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE4);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE5), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE5);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE6), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE6);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE7), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE7);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE8), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE8);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE9), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE9);
                serviceData["1"].AddParameter(nameof(this.SelectItem.TEXT_VALUE10), DbType.NVarChar, 200, this.SelectItem.TEXT_VALUE10);
                serviceData["1"].AddParameter(nameof(this.SelectItem.INT_VALUE1), DbType.NVarChar, 200, this.SelectItem.INT_VALUE1);
                serviceData["1"].AddParameter(nameof(this.SelectItem.INT_VALUE2), DbType.NVarChar, 200, this.SelectItem.INT_VALUE2);
                serviceData["1"].AddParameter(nameof(this.SelectItem.INT_VALUE3), DbType.NVarChar, 200, this.SelectItem.INT_VALUE3);
                serviceData["1"].AddParameter(nameof(this.SelectItem.INT_VALUE4), DbType.NVarChar, 200, this.SelectItem.INT_VALUE4);
                serviceData["1"].AddParameter(nameof(this.SelectItem.INT_VALUE5), DbType.NVarChar, 200, this.SelectItem.INT_VALUE5);
                serviceData["1"].AddParameter(nameof(this.SelectItem.NUMBER_VALUE1), DbType.NVarChar, 200, this.SelectItem.NUMBER_VALUE1);
                serviceData["1"].AddParameter(nameof(this.SelectItem.NUMBER_VALUE2), DbType.NVarChar, 200, this.SelectItem.NUMBER_VALUE2);
                serviceData["1"].AddParameter(nameof(this.SelectItem.NUMBER_VALUE3), DbType.NVarChar, 200, this.SelectItem.NUMBER_VALUE3);
                serviceData["1"].AddParameter(nameof(this.SelectItem.NUMBER_VALUE4), DbType.NVarChar, 200, this.SelectItem.NUMBER_VALUE4);
                serviceData["1"].AddParameter(nameof(this.SelectItem.NUMBER_VALUE5), DbType.NVarChar, 200, this.SelectItem.NUMBER_VALUE5);
                serviceData["1"].AddParameter(nameof(this.SelectItem.DATETIME_VALUE1), DbType.NVarChar, 200, this.SelectItem.DATETIME_VALUE1);
                serviceData["1"].AddParameter(nameof(this.SelectItem.DATETIME_VALUE2), DbType.NVarChar, 200, this.SelectItem.DATETIME_VALUE2);
                serviceData["1"].AddParameter(nameof(this.SelectItem.DATETIME_VALUE3), DbType.NVarChar, 200, this.SelectItem.DATETIME_VALUE3);
                serviceData["1"].AddParameter(nameof(this.SelectItem.DATETIME_VALUE4), DbType.NVarChar, 200, this.SelectItem.DATETIME_VALUE4);
                serviceData["1"].AddParameter(nameof(this.SelectItem.DATETIME_VALUE5), DbType.NVarChar, 200, this.SelectItem.DATETIME_VALUE5);
                serviceData["1"].AddParameter(nameof(this.SelectItem.INACTIVE_DATE), DbType.DateTime, 0, this.SelectItem.INACTIVE_DATE);
                serviceData["1"].AddParameter("USER_ID", DbType.Int, 3, this.UserClaim("Account.USER_ID").ToInt());

                response = serviceData.ServiceRequest(serviceData);

                if (response.Status == Status.OK)
                {
                    if (response.DataSet != null && response.DataSet.DataTables.Count > 0 && response.DataSet.DataTables[0].DataRows.Count > 0 && this.SelectItem != null && this.SelectItem.CLASS_ID == null)
                    {
                        value = response.DataSet.DataTables[0].DataRows[0].String("Value");

                        if (value != null)
                            this.SelectItem.CLASS_ID = value.ToInt();
                    }

                    this.ToastShow("Completed", $"{this.GetAttribute("Title")} registered successfully.", Alert.ToastDuration.Long);
                }
                else
                {
                    if (response.Message != null)
                    {
                        this.ModalShow("Warning", response.Message, new() { { "Ok", Btn.Warning } }, null);
                    }
                }
            }
            catch (Exception e)
            {
                this.ModalShow("An Exception has occurred.", e.Message, new() { { "Ok", Btn.Danger } }, null);
            }
            finally
            {
                this.A003ViewModel.IsBusy = false;

                if (response != null && response.Status == Status.OK)
                {
                    this.Search();
                    this.StateHasChanged();
                }
            }
        }

        private void Delete()
        {
            this.ModalShow($"Question", "Do you want to delete?", new() { { "Delete", Btn.Danger }, { "Cancel", Btn.Primary } }, EventCallback.Factory.Create<string>(this, DeleteAction));
        }
        private void DeleteAction(string action)
        {
            Response? response;

            response = null;

            try
            {
                if (action != "Delete") return;

                if (this.A003ViewModel.IsBusy) return;

                if (this.SelectItem == null) return;

                this.A003ViewModel.IsBusy = true;

                if (this.SelectItem.CLASS_ID == null || this.SelectItem.CLASS_ID <= 0)
                {
                    this.ToastShow("Warning", $"Please select a {this.GetAttribute("Title")} to delete.", Alert.ToastDuration.Long);
                    return;
                }

                ServiceData serviceData = new()
                {
                    TransactionScope = true,
                    Token = this.UserClaim("Token")
                };
                serviceData["1"].CommandText = this.GetAttribute("Delete");
                serviceData["1"].AddParameter(nameof(this.SelectItem.CLASS_ID), DbType.Int, 3, this.SelectItem.CLASS_ID);
                serviceData["1"].AddParameter("USER_ID", DbType.Int, 3, this.UserClaim("Account.USER_ID").ToInt());

                response = serviceData.ServiceRequest(serviceData);

                if (response.Status == Status.OK)
                {
                    this.New();
                    this.ToastShow("Completed", $"{this.GetAttribute("Title")} deleted successfully.", Alert.ToastDuration.Long);
                }
                else
                {
                    if (response.Message != null)
                    {
                        this.ModalShow("Warning", response.Message, new() { { "Ok", Btn.Warning } }, null);
                    }
                }
            }
            catch (Exception e)
            {
                this.ModalShow("An Exception has occurred.", e.Message, new() { { "Ok", Btn.Danger } }, null);
            }
            finally
            {
                this.A003ViewModel.IsBusy = false;

                if (response != null && response.Status == Status.OK)
                {
                    this.Search();
                    this.StateHasChanged();
                }
            }
        }
        #endregion


        #region Event
        private void SearchKeydown(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                this.OnSearch();
            }
        }

        private void RowModify(CommonClassModel item)
        {
            this.SelectItem = new()
            {
                CLASS_ID = item.CLASS_ID,
                CLASS_NAME = item.CLASS_NAME,
                KEY_VALUE = item.KEY_VALUE,
                TEXT_VALUE1 = item.TEXT_VALUE1,
                TEXT_VALUE2 = item.TEXT_VALUE2,
                TEXT_VALUE3 = item.TEXT_VALUE3,
                TEXT_VALUE4 = item.TEXT_VALUE4,
                TEXT_VALUE5 = item.TEXT_VALUE5,
                TEXT_VALUE6 = item.TEXT_VALUE6,
                TEXT_VALUE7 = item.TEXT_VALUE7,
                TEXT_VALUE8 = item.TEXT_VALUE8,
                TEXT_VALUE9 = item.TEXT_VALUE9,
                TEXT_VALUE10 = item.TEXT_VALUE10,
                INT_VALUE1 = item.INT_VALUE1,
                INT_VALUE2 = item.INT_VALUE2,
                INT_VALUE3 = item.INT_VALUE3,
                INT_VALUE4 = item.INT_VALUE4,
                INT_VALUE5 = item.INT_VALUE5,
                NUMBER_VALUE1 = item.NUMBER_VALUE1,
                NUMBER_VALUE2 = item.NUMBER_VALUE2,
                NUMBER_VALUE3 = item.NUMBER_VALUE3,
                NUMBER_VALUE4 = item.NUMBER_VALUE4,
                NUMBER_VALUE5 = item.NUMBER_VALUE5,
                DATETIME_VALUE1 = item.DATETIME_VALUE1,
                DATETIME_VALUE2 = item.DATETIME_VALUE2,
                DATETIME_VALUE3 = item.DATETIME_VALUE3,
                DATETIME_VALUE4 = item.DATETIME_VALUE4,
                DATETIME_VALUE5 = item.DATETIME_VALUE5,
                INACTIVE_DATE = item.INACTIVE_DATE,
            };
        }

        private void Copy()
        {
            if (this.SelectItem != null)
            {
                this.SelectItem.CLASS_ID = null;
            }
        }
        #endregion
    }
}
