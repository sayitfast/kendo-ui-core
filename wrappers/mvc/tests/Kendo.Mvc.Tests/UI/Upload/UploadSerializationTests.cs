namespace Kendo.Mvc.UI.Tests.Upload
{
    using Moq;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Routing;
    using Kendo.Mvc;
    using Kendo.Mvc.Infrastructure;
    using Kendo.Mvc.UI;
    using Xunit;

    public class UploadSerializationTests
    {
        private readonly Upload upload;
        private readonly Mock<TextWriter> textWriter;
        private readonly Mock<ILocalizationService> localizationServiceMock;
        private string output;

        public UploadSerializationTests()
        {
            textWriter = new Mock<TextWriter>();
            textWriter.Setup(tw => tw.Write(It.IsAny<string>())).Callback<string>(s => output += s);

            var urlGeneratorMock = new Mock<IUrlGenerator>();
            urlGeneratorMock.Setup(g => g.Generate(It.IsAny<RequestContext>(), It.IsAny<INavigatable>()))
                .Returns<RequestContext, INavigatable>(
                    (context, navigatable) => navigatable.ControllerName + "/" + navigatable.ActionName
                );

            localizationServiceMock = new Mock<ILocalizationService>();
            localizationServiceMock.SetupGet(ls => ls.IsDefault).Returns(true);
            localizationServiceMock.Setup(l => l.All()).Returns(() =>
                new Dictionary<string, string> { { "Remove", "Entfernen" } });

            upload = UploadTestHelper.CreateUpload(urlGeneratorMock.Object, localizationServiceMock.Object);
            upload.Name = "Upload";
        }

        [Fact]
        public void Default_configuration_outputs_empty_kendoUpload_init()
        {
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("jQuery('#Upload').kendoUpload();");
        }

        [Fact]
        public void Enabled_state_serialized_when_false()
        {
            upload.Enabled = false;
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{enabled:false}");
        }

        [Fact]
        public void Multiple_state_serialized_when_false()
        {
            upload.Multiple = false;
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{multiple:false}");
        }

        [Fact]
        public void ShowFileList_should_be_serialized_when_false()
        {
            upload.ShowFileList = false;
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{showFileList:false}");
        }

        [Fact]
        public void Async_settings_should_be_serialized_if_Save_action_is_set()
        {
            upload.Async.Save.ActionName = "Index";
            upload.Async.Save.ControllerName = "Home";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{async:{\"saveUrl\":\"Home/Index\",\"autoUpload\":true}}");
        }

        [Fact]
        public void Async_SaveField_should_be_serialized_when_set()
        {
            upload.Async.Save.ActionName = "Index";
            upload.Async.Save.ControllerName = "Home";
            upload.Async.SaveField = "attachments";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{async:{\"saveUrl\":\"Home/Index\",\"saveField\":\"attachments\",\"autoUpload\":true}}");
        }

        [Fact]
        public void Remove_action_should_be_serialized()
        {
            upload.Async.Save.ActionName = "Index";
            upload.Async.Save.ControllerName = "Home";
            upload.Async.Remove.ActionName = "Remove";
            upload.Async.Remove.ControllerName = "Home";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{async:{\"saveUrl\":\"Home/Index\",\"removeUrl\":\"Home/Remove\",\"autoUpload\":true}}");
        }

        [Fact]
        public void Remove_action_should_not_be_serialized_when_Save_is_not_set()
        {
            upload.Async.Remove.ActionName = "Remove";
            upload.Async.Remove.ControllerName = "Home";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("");
        }

        [Fact]
        public void AutoUpload_should_be_serialized_when_false()
        {
            upload.Async.Save.ActionName = "Index";
            upload.Async.Save.ControllerName = "Home";
            upload.Async.AutoUpload = false;
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{async:{\"saveUrl\":\"Home/Index\",\"autoUpload\":false}}");
        }

        [Fact]
        public void AutoUpload_should_not_be_serialized_when_Save_is_not_set()
        {
            upload.Async.AutoUpload = false;
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("jQuery('#Upload').kendoUpload();");
        }

        [Fact]
        public void OnLoad_client_side_event_serialized()
        {
            upload.ClientEvents.OnLoad.HandlerName = "loadHandler";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{load:loadHandler}");
        }

        [Fact]
        public void OnSelect_client_side_event_serialized()
        {
            upload.ClientEvents.OnSelect.HandlerName = "selectHandler";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{select:selectHandler}");
        }

        [Fact]
        public void OnUpload_client_side_event_serialized()
        {
            upload.ClientEvents.OnUpload.HandlerName = "uploadHandler";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{upload:uploadHandler}");
        }

        [Fact]
        public void OnSuccess_client_side_event_serialized()
        {
            upload.ClientEvents.OnSuccess.HandlerName = "successHandler";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{success:successHandler}");
        }

        [Fact]
        public void OnError_client_side_event_serialized()
        {
            upload.ClientEvents.OnError.HandlerName = "errorHandler";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{error:errorHandler}");
        }

        [Fact]
        public void OnComplete_client_side_event_serialized()
        {
            upload.ClientEvents.OnComplete.HandlerName = "completeHandler";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{complete:completeHandler}");
        }

        [Fact]
        public void OnCancel_client_side_event_serialized()
        {
            upload.ClientEvents.OnCancel.HandlerName = "cancelHandler";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{cancel:cancelHandler}");
        }

        [Fact]
        public void OnRemove_client_side_event_serialized()
        {
            upload.ClientEvents.OnRemove.HandlerName = "removeHandler";
            upload.WriteInitializationScript(textWriter.Object);

            output.ShouldContain("{remove:removeHandler}");
        }

        [Fact]
        public void Localization_is_serialized()
        {
            localizationServiceMock.SetupGet(ls => ls.IsDefault).Returns(false);
            upload.WriteInitializationScript(textWriter.Object);
            output.ShouldContain("{localization:{\"remove\":\"Entfernen\"}}");
        }
    }
}
