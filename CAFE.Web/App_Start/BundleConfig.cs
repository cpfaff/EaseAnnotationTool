using System.Web.Optimization;

namespace CAFE.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/global").Include(
				"~/Scripts/app/global/formSubmit.js"
				));

            //FOR ADMIN
            bundles.Add(new ScriptBundle("~/bundles/admin/jquery").Include(
                "~/Scripts/libs/jquery/jquery.min.js",
                "~/Scripts/libs/lodash/lodash.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/bootstrap").Include(
                "~/Scripts/libs/bootstrap/js/bootstrap.min.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/angular").Include(
                "~/Scripts/libs/angular/angular.min.js",
                "~/Scripts/libs/angular-route/angular-route.min.js",
                "~/Scripts/libs/angular-ui-router/angular-ui-router.min.js",
                "~/Scripts/libs/angular-animate/angular-animate.min.js",
                "~/Scripts/libs/angular-aria/angular-aria.min.js",
                "~/Scripts/libs/angular-sanitize/angular-sanitize.min.js",
                "~/Scripts/libs/angular-messages/angular-messages.min.js",
                "~/Scripts/libs/angular-material/angular-material.min.js",
                "~/Scripts/libs/angular-material/modules/js/select/select.min.js",
                "~/Scripts/libs/angular-ui-select/select.min.js",
                //"~/Scripts/libs/angular-material-icons/angular-material-icons.min.js",
                "~/Scripts/libs/md-data-table/md-data-table.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/app").Include(
                "~/Scripts/libs/ui-grid-master/ui-grid.min.js",

                "~/Scripts/app/admin/core/annotation/annotation.module.js",
                "~/Scripts/app/admin/core/annotation/annotation.service.js",

                "~/Scripts/app/admin/core/user/user.module.js",
                "~/Scripts/app/admin/core/user/user.service.js",

                "~/Scripts/app/admin/core/group/group.module.js",
                "~/Scripts/app/admin/core/group/group.service.js",

                "~/Scripts/app/admin/core/masterdata/masterdata.module.js",
                "~/Scripts/app/admin/core/masterdata/masterdata.service.js",

                "~/Scripts/app/admin/core/core.module.js",

                "~/Scripts/app/admin/users/user-list/user-list.module.js",
                "~/Scripts/app/admin/users/user-list/user-list.component.js",

                "~/Scripts/app/admin/users/user-acceptance-list/user-acceptance-list.module.js",
                "~/Scripts/app/admin/users/user-acceptance-list/user-acceptance-list.component.js",

                "~/Scripts/app/admin/groups/group-list/group-list.module.js",
                "~/Scripts/app/admin/groups/group-list/group-list.component.js",

                "~/Scripts/app/admin/annotation/annotation-list.module.js",
                "~/Scripts/app/admin/annotation/annotation-list.component.js",

                "~/Scripts/app/admin/masterdata/masterdata.module.js",
                "~/Scripts/app/admin/masterdata/masterdata.component.js",

                "~/Scripts/app/admin/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/users-dashboard").Include(
                "~/Scripts/libs/angular-material-fileinput-master/lf-ng-md-file-input.js",

                "~/Scripts/libs/ng-tags-input/ng-tags-input.js",

                "~/Scripts/app/users-dashboard/core/files/files.module.js",
                "~/Scripts/app/users-dashboard/core/files/files.service.js",

                "~/Scripts/app/users-dashboard/core/acceptRequests/acceptRequests.module.js",
                "~/Scripts/app/users-dashboard/core/acceptRequests/acceptRequests.service.js",

                "~/Scripts/app/users-dashboard/core/unacceptedUsers/unacceptedUsers.module.js",
                "~/Scripts/app/users-dashboard/core/unacceptedUsers/unacceptedUsers.service.js",

                "~/Scripts/app/users-dashboard/core/annotationItems/annotationItems.module.js",
                "~/Scripts/app/users-dashboard/core/annotationItems/annotationItems.service.js",

                "~/Scripts/app/users-dashboard/core/core.module.js",
                
                "~/Scripts/app/users-dashboard/acceptRequests/accept-requests.module.js",
                "~/Scripts/app/users-dashboard/acceptRequests/accept-requests.component.js",

                "~/Scripts/app/users-dashboard/unacceptedUsers/unaccepted-users.module.js",
                "~/Scripts/app/users-dashboard/unacceptedUsers/unaccepted-users.component.js",

                "~/Scripts/app/users-dashboard/files/files-list/files-list.module.js",
                "~/Scripts/app/users-dashboard/files/files-list/files-list.component.js",

                 "~/Scripts/app/users-dashboard/annotationItems/annotationItems.module.js",
                "~/Scripts/app/users-dashboard/annotationItems/annotationItems.component.js",

                "~/Scripts/app/users-dashboard/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/incoming-requests").Include(

                "~/Scripts/app/access-requests/incoming-requests/core/incoming-requests/incoming-requests.module.js",
                "~/Scripts/app/access-requests/incoming-requests/core/incoming-requests/incoming-requests.service.js",

                "~/Scripts/app/access-requests/incoming-requests/core/core.module.js",

                "~/Scripts/app/access-requests/incoming-requests/incoming-requests/incoming-requests-list/incoming-requests-list.module.js",
                "~/Scripts/app/access-requests/incoming-requests/incoming-requests/incoming-requests-list/incoming-requests-list.component.js",

                "~/Scripts/app/access-requests/incoming-requests/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/outgoing-requests").Include(

                "~/Scripts/app/access-requests/outgoing-requests/core/outgoing-requests/outgoing-requests.module.js",
                "~/Scripts/app/access-requests/outgoing-requests/core/outgoing-requests/outgoing-requests.service.js",

                "~/Scripts/app/access-requests/outgoing-requests/core/core.module.js",

                "~/Scripts/app/access-requests/outgoing-requests/outgoing-requests/outgoing-requests-list/outgoing-requests-list.module.js",
                "~/Scripts/app/access-requests/outgoing-requests/outgoing-requests/outgoing-requests-list/outgoing-requests-list.component.js",

                "~/Scripts/app/access-requests/outgoing-requests/app.js"));

            bundles.Add(new StyleBundle("~/bundles/users-dashboard/css").Include(
                       "~/Scripts/libs/ng-tags-input/ng-tags-input.bootstrap.css",
                       "~/Scripts/libs/ng-tags-input/ng-tags-input.css",
                       "~/Scripts/libs/angular-material-fileinput-master/lf-ng-md-file-input.css"
                       ));


            bundles.Add(new StyleBundle("~/Content/admin/css").Include(
                      "~/Scripts/libs/bootstrap/css/bootstrap.css",
                      "~/Scripts/libs/font-awesome/css/font-awesome.min.css",
                      "~/Scripts/libs/angular-material/angular-material.min.css",
                      "~/Scripts/libs/angular-material/modules/js/select/select.min.css",
                      "~/Scripts/libs/angular-ui-select/select.min.css",
                      //"~/Scripts/libs/angular-material-icons/angular-material-icons.css",
                      "~/Scripts/libs/md-data-table/md-data-table.min.css",
                      "~/Content/icons.css",
                      "~/Content/admin.css",
                      "~/Scripts/libs/ui-grid-master/ui-grid.min.css"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/search/angular").Include(
                "~/Scripts/libs/angular/angular.min.js",
                "~/Scripts/libs/angular-route/angular-route.min.js",
                "~/Scripts/libs/angular-animate/angular-animate.min.js",
                "~/Scripts/libs/angular-sanitize/angular-sanitize.min.js",
                "~/Scripts/libs/angular-ui-select/select.min.js",
                "~/Scripts/libs/angular-aria/angular-aria.min.js",
                "~/Scripts/libs/angular-messages/angular-messages.min.js",
                "~/Scripts/libs/angular-material/angular-material.min.js",
                "~/Scripts/libs/md-data-table/md-data-table.min.js",
                "~/Scripts/libs/np-autocomplete/np-autocomplete.min.js",
                "~/Scripts/libs/ui-bootstrap/ui-bootstrap-tpls-2.2.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/search/app").Include(
                "~/Scripts/app/search/app.js",
                "~/Scripts/app/search/core/core.module.js",
                "~/Scripts/app/search/core/access-request/access-request.module.js",
                "~/Scripts/app/search/core/access-request/access-request.service.js",
                "~/Scripts/app/search/access-requests/access-request-dialog/access-request-dialog.module.js",
                "~/Scripts/app/search/access-requests/access-request-dialog/access-request-dialog.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/search/css").Include(
                "~/Scripts/libs/angular-ui-select/select.min.css",
                "~/Scripts/libs/np-autocomplete/np-autocomplete.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/common/angular").Include(
                "~/Scripts/libs/angular/angular.min.js",
                "~/Scripts/libs/angular-route/angular-route.min.js",
                "~/Scripts/libs/angular-ui-router/angular-ui-router.min.js",
                "~/Scripts/libs/angular-animate/angular-animate.min.js",
                "~/Scripts/libs/angular-aria/angular-aria.min.js",
                "~/Scripts/libs/angular-sanitize/angular-sanitize.min.js",
                "~/Scripts/libs/angular-messages/angular-messages.min.js",
                "~/Scripts/libs/angular-material/angular-material.min.js",
                "~/Scripts/libs/angular-material/modules/js/select/select.min.js",
                "~/Scripts/libs/md-data-table/md-data-table.min.js",
                "~/Scripts/libs/ui-bootstrap/ui-bootstrap-tpls-2.2.0.js",
                "~/Scripts/libs/angular-slider/rzslider.min.js",
                "~/Scripts/libs/leaflet/leaflet.js",
                "~/Scripts/libs/leaflet/Leaflet.Editable.js",
                "~/Scripts/libs/leaflet/Path.Drag.js",
                "~/Scripts/libs/angular-leaflet-directive/angular-leaflet-directive.js"
                ));



            bundles.Add(new StyleBundle("~/bundles/annotation/css").Include(
               "~/Scripts/libs/angular-ui-tree/angular-ui-tree.min.css",
               "~/Scripts/libs/mdPickers/dist/mdPickers.min.css",
               "~/Scripts/libs/angular-slider/rzslider.min.css",
               "~/Scripts/libs/leaflet/leaflet.css",
               "~/Scripts/libs/angular-material-fileinput-master/lf-ng-md-file-input.css"
               ));


            bundles.Add(new ScriptBundle("~/bundles/annotation/app").Include(
                //**** Additional libraries ****
                "~/Scripts/libs/angular-ui-tree/angular-ui-tree.min.js",
                "~/Scripts/moment.js",
                "~/Scripts/libs/mdPickers/dist/mdPickers.min.js",
                "~/Scripts/libs/angular-material-fileinput-master/lf-ng-md-file-input.js",
                "~/Scripts/libs/mgrs/dist/mgrs.js",
                "~/Scripts/mimetype.js",
                    //**** End Additional libraries ****

                    //**** Core ****

                    //Time Core
                    "~/Scripts/app/annotation-item/core/time/time.module.js",
                    "~/Scripts/app/annotation-item/core/time/time.service.js",
                    //Time Core

                    //Space Core
                    "~/Scripts/app/annotation-item/core/space/space.module.js",
                    "~/Scripts/app/annotation-item/core/space/space.service.js",
                    //Space Core

                    //Biome Core
                    "~/Scripts/app/annotation-item/core/biome/biome.module.js",
                    "~/Scripts/app/annotation-item/core/biome/biome.service.js",
                    //Biome Core

                    //Organism Core
                    "~/Scripts/app/annotation-item/core/organism/organism.module.js",
                    "~/Scripts/app/annotation-item/core/organism/organism.service.js",
                    //Organism Core

                    //Chemical Core
                    "~/Scripts/app/annotation-item/core/chemical/chemical.module.js",
                    "~/Scripts/app/annotation-item/core/chemical/chemical.service.js",
                    //Chemical Core

                    //Method Core
                    "~/Scripts/app/annotation-item/core/method/method.module.js",
                    "~/Scripts/app/annotation-item/core/method/method.service.js",
                    //Method Core

                "~/Scripts/app/annotation-item/core/model/annotationModel.module.js",
                "~/Scripts/app/annotation-item/core/model/annotationModel.service.js",
                "~/Scripts/app/annotation-item/sphere/sphere.module.js",
                "~/Scripts/app/annotation-item/sphere/sphere.controller.js",
                "~/Scripts/app/annotation-item/sphere/filters/sphereFilters.js",
                "~/Scripts/app/annotation-item/space/space.module.js",
                "~/Scripts/app/annotation-item/space/space.controller.js",
                "~/Scripts/app/annotation-item/time/time.module.js",
                "~/Scripts/app/annotation-item/time/time.service.js",

                    "~/Scripts/app/annotation-item/core/core.module.js",

                //**** End Core ****

                //Common
                "~/Scripts/app/annotation-item/common/common.module.js",
                //Common

                //Time
                "~/Scripts/app/annotation-item/time/time.module.js",
                "~/Scripts/app/annotation-item/time/time.components.js",
                //Time

                //Biome
                "~/Scripts/app/annotation-item/biome/_biome.module.js",
                "~/Scripts/app/annotation-item/biome/_biome.components.js",
                "~/Scripts/app/annotation-item/biome/biome.biome.components.js",
                "~/Scripts/app/annotation-item/biome/biome.physiognomy.components.js",
                //Biome

                //Organism
                "~/Scripts/app/annotation-item/organism/organism.module.js",
                //Organism

                //Organism
                "~/Scripts/app/annotation-item/chemical/chemical.module.js",
                //Organism

                //Method
                "~/Scripts/app/annotation-item/method/method.module.js",
                //Method

                //Files
                "~/Scripts/app/annotation-item/files/files.module.js",
                //Files

                "~/Scripts/app/annotation-item/process/process.module.js",
                "~/Scripts/app/annotation-item/process/process.controller.js",
                "~/Scripts/app/annotation-item/app.js"));
        }
    }
}
