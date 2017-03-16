namespace Sitecore.Support.ContentTesting.Pipelines.RenderLayout
{
  using Sitecore.Analytics.Configuration;
  using Sitecore.Analytics.Data.Items;
  using Sitecore.Analytics.Testing;
  using Sitecore.ContentTesting.Configuration;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines.RenderLayout;

  public class EvaluateTestExposure : Sitecore.ContentTesting.Pipelines.RenderLayout.EvaluateTestExposure
  {
    public new void Process(RenderLayoutArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      if (((AnalyticsSettings.Enabled && ((Context.Site == null) || (Context.Site.Name != "shell"))) && (Context.Item != null)) && Settings.IsAutomaticContentTestingEnabled)
      {
        var configuration = FindTestForItem(Context.Item, Context.Device.ID);
        if (configuration != null)
        {
          var testItem = configuration.TestDefinitionItem;
          var testset = TestManager.GetTestSet(new TestDefinitionItem[] { testItem }, Context.Item, Context.Device.ID);
          
          if (Context.Database.GetItem(new ID(testset.Id)) == null)
          {
            // log error and abort execution
            Log.Error($"Failed to evaluate test exposure due to missing item: {testset.Id}, Database: {Context.Database.Name}, configuration.TestDefinitionItem: {testItem?.ID}", this);
            
            return;
          }
        }
      }

      base.Process(args);
    }
  }
}