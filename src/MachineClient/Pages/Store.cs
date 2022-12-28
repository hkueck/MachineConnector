using MachineClient.Model;

namespace MachineClient.Pages;

public partial class Store
{
    private StoreModel[]? _stores;

    protected override void OnInitialized()
    {
        _stores = new StoreModel[] {new StoreModel {Name = "Lager 1"}};
        base.OnInitialized();
    }
}