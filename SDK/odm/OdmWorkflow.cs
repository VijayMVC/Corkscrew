using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.odm
{
    /// <summary>
    /// ODM for Workflow system
    /// </summary>
    internal class OdmWorkflow : OdmBase
    {

        private CSFarm _farm = null;
        private CSSite _site = null;
        private OdmSite _odmSite = null;

        public OdmWorkflow(CSFarm farm, CSSite site = null) 
            : base(null)                                            // change the param to site if we want to ever have tables in the SiteDB
        {
            _farm = farm;
            _site = site;
            _odmSite = new OdmSite();
        }

        public bool CreateWorkflowDefinition(CSWorkflowDefinition definition)
        {
            return base.CommitChanges
            (
                "WrkflwDefnCreateWorkflow",
                new Dictionary<string, object>()
                {
                    { "@Id", definition.Id },
                    { "@Name", definition.Name },
                    { "@Description", definition.Description },
                    { "@DefaultAssociationData", definition.DefaultAssociationData },
                    { "@allow_manual_start", definition.AllowManualStart },
                    { "@start_on_create", definition.StartOnCreate },
                    { "@start_on_modify", definition.StartOnModify },
                    { "@is_enabled", definition.IsEnabled },
                    { "@CreatedBy", definition.CreatedBy.Id },
                    { "@Created", definition.Created }
                }
            );
        }

        public bool CreateWorkflowManifest(CSWorkflowManifest manifest)
        {
            return base.CommitChanges
            (
                "WrkflwManifestCreateManifest",
                new Dictionary<string, object>()
                {
                    { "@ManifestId", manifest.Id },
                    { "@WorkflowDefinitionId", manifest.WorkflowDefinition.Id },
                    { "@WorkflowEngineName", Enum.GetName(typeof(WorkflowEngineEnum), manifest.WorkflowEngine) },
                    { "@OutputAssemblyName", manifest.OutputAssemblyName },
                    { "@WorkflowClassName", manifest.WorkflowClassName },
                    { "@always_compile", manifest.AlwaysCompile },
                    { "@cache_compile_results", manifest.CacheCompileResults },
                    { "@CreatedBy", manifest.CreatedBy.Id },
                    { "@Created", manifest.Created }
                }
            );
        }

        public bool CreateWorkflowManifestItem(CSWorkflowManifestItem item)
        {
            return base.CommitChanges
            (
                "WrkflwManifestAddManifestItem",
                new Dictionary<string, object>()
                {
                    { "@ManifestItemId", item.Id },
                    { "@WorkflowDefinitionId", item.WorkflowDefinition.Id },
                    { "@WorkflowManifestId", item.WorkflowManifest.Id },
                    { "@Filename", item.Filename },
                    { "@FilenameExtension", item.FilenameExtension },
                    { "@ItemType", (int)item.ItemType },
                    { "@BuildRelativeFolder", item.BuildtimeRelativeFolder },
                    { "@RuntimeFolder", item.RuntimeRelativeFolder },
                    { "@RequiredForExecution", item.RequiredForExecution },
                    { "@ContentStream", item.FileContent },
                    { "@CreatedBy", item.CreatedBy.Id },
                    { "@Created", item.Created }
                }
            );
        }

        public bool CreateWorkflowAssociation(CSWorkflowAssociation assoc)
        {
            return base.CommitChanges
            (
                "WrkflwAssnCreateAssociation",
                new Dictionary<string, object>()
                {
                    { "@Id", assoc.Id },
                    { "@WorkflowDefinitionId", assoc.WorkflowDefinition.Id },
                    { "@Name", assoc.Name },
                    { "@AssociationData", assoc.CustomAssociationInformation },
                    { "@FarmId", assoc.Farm.Id },
                    { "@SiteId", ((assoc.Site != null) ? assoc.Site.Id : Guid.Empty) },
                    { "@DirectoryId", ((assoc.AssociatedEntity != null) ? assoc.AssociatedEntity.Id : Guid.Empty) },
                    { "@allow_manual_start", assoc.AllowManualStart },
                    { "@start_on_create", assoc.StartOnCreate },
                    { "@start_on_modify", assoc.StartOnModify },
                    { "@is_enabled", assoc.IsEnabled },
                    { "@CreatedBy", assoc.CreatedBy.Id },
                    { "@Created", assoc.Created }
                }
            );
        }

        public bool UpdateEventSubscriptions(CSWorkflowDefinition definition, CSWorkflowTriggerEvents events)
        {
            return base.CommitChanges
            (
                "WrkflwDefnAllowEvents",
                new Dictionary<string, object>()
                {
                    { "@WorkflowDefinitionId", definition.Id },
                    { "@allow_event_farm_created", events.Has(WorkflowTriggerEventNamesEnum.farm_created) },
                    { "@allow_event_farm_modified", events.Has(WorkflowTriggerEventNamesEnum.farm_modified) },
                    { "@allow_event_farm_deleted", events.Has(WorkflowTriggerEventNamesEnum.farm_deleted) },
                    { "@allow_event_site_created", events.Has(WorkflowTriggerEventNamesEnum.site_created) },
                    { "@allow_event_site_modified", events.Has(WorkflowTriggerEventNamesEnum.site_modified) },
                    { "@allow_event_site_deleted", events.Has(WorkflowTriggerEventNamesEnum.site_deleted) },
                    { "@allow_event_directory_created", events.Has(WorkflowTriggerEventNamesEnum.directory_created) },
                    { "@allow_event_directory_modified", events.Has(WorkflowTriggerEventNamesEnum.directory_modified) },
                    { "@allow_event_directory_deleted", events.Has(WorkflowTriggerEventNamesEnum.directory_deleted) },
                    { "@allow_event_file_created", events.Has(WorkflowTriggerEventNamesEnum.file_created) },
                    { "@allow_event_file_modified", events.Has(WorkflowTriggerEventNamesEnum.file_modified) },
                    { "@allow_event_file_deleted", events.Has(WorkflowTriggerEventNamesEnum.file_deleted) },
                    { "@allow_event_catch_bubbledevents", definition.AllowProcessingBubbledTriggers },
                    { "@ModifiedBy", definition.ModifiedBy.Id },
                    { "@Modified", definition.Modified }
                }
            );
        }

        public bool UpdateEventSubscriptions(CSWorkflowAssociation association, CSWorkflowTriggerEvents events)
        {
            return base.CommitChanges
            (
                "WrkflwAssnSubscribeEvents",
                new Dictionary<string, object>()
                {
                    { "@WorkflowAssociationId", association.Id },
                    { "@subscribe_event_farm_created", events.Has(WorkflowTriggerEventNamesEnum.farm_created) },
                    { "@subscribe_event_farm_modified", events.Has(WorkflowTriggerEventNamesEnum.farm_modified) },
                    { "@subscribe_event_farm_deleted", events.Has(WorkflowTriggerEventNamesEnum.farm_deleted) },
                    { "@subscribe_event_site_created", events.Has(WorkflowTriggerEventNamesEnum.site_created) },
                    { "@subscribe_event_site_modified", events.Has(WorkflowTriggerEventNamesEnum.site_modified) },
                    { "@subscribe_event_site_deleted", events.Has(WorkflowTriggerEventNamesEnum.site_deleted) },
                    { "@subscribe_event_directory_created", events.Has(WorkflowTriggerEventNamesEnum.directory_created) },
                    { "@subscribe_event_directory_modified", events.Has(WorkflowTriggerEventNamesEnum.directory_modified) },
                    { "@subscribe_event_directory_deleted", events.Has(WorkflowTriggerEventNamesEnum.directory_deleted) },
                    { "@subscribe_event_file_created", events.Has(WorkflowTriggerEventNamesEnum.file_created) },
                    { "@subscribe_event_file_modified", events.Has(WorkflowTriggerEventNamesEnum.file_modified) },
                    { "@subscribe_event_file_deleted", events.Has(WorkflowTriggerEventNamesEnum.file_deleted) },
                    { "@subscribe_event_catch_bubbledevents", association.AllowProcessingBubbledTriggers },
                    { "@ModifiedBy", association.ModifiedBy.Id },
                    { "@Modified", association.Modified }
                }
            );
        }

        public bool SaveChanges(CSWorkflowManifest manifest, bool isCompileResult = false)
        {
            return base.CommitChanges
            (
                "WrkflwManifestSaveManifest",
                new Dictionary<string, object>()
                {
                    { "@ManifestId", manifest.Id },
                    { "@WorkflowEngineName", Enum.GetName(typeof(WorkflowEngineEnum), manifest.WorkflowEngine) },
                    { "@OutputAssemblyName", manifest.OutputAssemblyName },
                    { "@WorkflowClassName", manifest.WorkflowClassName },

                    { "@buildAssemblyTitle", manifest.BuildAssemblyTitle },
                    { "@buildAssemblyDescription", manifest.BuildAssemblyDescription },
                    { "@buildAssemblyCompany", manifest.BuildAssemblyCompany },
                    { "@buildAssemblyProduct", manifest.BuildAssemblyProduct },
                    { "@buildAssemblyCopyright", manifest.BuildAssemblyCopyright },
                    { "@buildAssemblyTrademark", manifest.BuildAssemblyTrademark },
                    { "@buildAssemblyVersion", manifest.BuildAssemblyVersion.ToString(4) },
                    { "@buildAssemblyFileVersion", manifest.BuildAssemblyFileVersion.ToString(4) },

                    { "@always_compile", manifest.AlwaysCompile },
                    { "@cache_compile_results", manifest.CacheCompileResults },
                    { "@is_compilation_result", isCompileResult },

                    { "@ModifiedBy", manifest.ModifiedBy.Id },
                    { "@Modified", manifest.Modified }
                }
            );
        }

        public bool SaveChanges(CSWorkflowManifestItem item)
        {
            return base.CommitChanges
            (
                "WrkflwManifestSaveManifestItem",
                new Dictionary<string, object>()
                {
                    { "@ManifestItemId", item.Id },
                    { "@Filename", item.Filename },
                    { "@FilenameExtension", item.FilenameExtension },
                    { "@ItemType", (int)item.ItemType },
                    { "@BuildRelativeFolder", item.BuildtimeRelativeFolder },
                    { "@RuntimeFolder", item.RuntimeRelativeFolder },
                    { "@RequiredForExecution", item.RequiredForExecution },
                    { "@ContentStream", item.FileContent },
                    { "@ModifiedBy", item.ModifiedBy.Id },
                    { "@Modified", item.Modified }
                }
            );
        }

        public bool SaveChanges(CSWorkflowDefinition definition)
        {
            return base.CommitChanges
            (
                "WrkflwDefnSaveChanges",
                new Dictionary<string, object>()
                {
                    { "@Id", definition.Id },
                    { "@Name", definition.Name },
                    { "@Description", definition.Description },
                    { "@DefaultAssociationData", definition.DefaultAssociationData },
                    { "@allow_manual_start", definition.AllowManualStart },
                    { "@start_on_create", definition.StartOnCreate },
                    { "@start_on_modify", definition.StartOnModify },
                    { "@is_enabled", definition.IsEnabled },
                    { "@ModifiedBy", definition.ModifiedBy.Id },
                    { "@Modified", definition.Modified }
                }
            );
        }

        public bool SaveChanges(CSWorkflowAssociation assoc)
        {
            return base.CommitChanges
            (
                "WrkflwAssnSaveAssociation",
                new Dictionary<string, object>()
                {
                    { "@Id", assoc.Id },
                    { "@Name", assoc.Name },
                    { "@AssociationData", assoc.CustomAssociationInformation },
                    { "@allow_manual_start", assoc.AllowManualStart },
                    { "@start_on_create", assoc.StartOnCreate },
                    { "@start_on_modify", assoc.StartOnModify },
                    { "@prevent_new_instances", assoc.PreventStartingNewInstances },
                    { "@is_enabled", assoc.IsEnabled },
                    { "@ModifiedBy", assoc.ModifiedBy.Id },
                    { "@Modified", assoc.Modified }
                }
            );
        }

        public bool UpdateInstanceState(CSWorkflowInstance instance)
        {
            return base.CommitChanges
            (
                "WrkflwInstUpdateStateChange",
                new Dictionary<string, object>()
                {
                    { "@Id", instance.Id },
                    { "@SqlPersistenceId", instance.WorkflowPeristenceId },
                    { "@State", instance.CurrentState },
                    { "@StateInfo", instance.InstanceInformation },
                    { "@Reason", instance.CompletedReason },
                    { "@ErrorMessage", instance.ErrorMessage },
                    { "@IsLoadedInRuntime", instance.IsLoadedInRuntime }
                }
            );
        }

        public bool DeleteWorkflowManifest(CSWorkflowManifest manifest)
        {
            return base.CommitChanges
            (
                "WrkflwManifestDeleteManifest",
                new Dictionary<string, object>()
                {
                    { "@WorkflowManifestId", manifest.Id }
                }
            );
        }

        public bool DeleteWorkflowManifestItem(CSWorkflowManifestItem item)
        {
            return base.CommitChanges
            (
                "WrkflwManifestDeleteManifestItem",
                new Dictionary<string, object>()
                {
                    { "@ManifestItemId", item.Id }
                }
            );
        }

        public bool DeleteWorkflowDefinition(CSWorkflowDefinition definition)
        {
            return base.CommitChanges
            (
                "WrkflwDefnDeleteById",
                new Dictionary<string, object>()
                {
                    { "@Id", definition.Id }
                }
            );
        }

        public bool DeleteAssociationsForWorkflowDefinition(CSWorkflowDefinition definition)
        {
            return base.CommitChanges
            (
                "WrkflwAssnDeleteByDefinitionId",
                new Dictionary<string, object>()
                {
                    { "@WorkflowDefinitionId", definition.Id }
                }
            );
        }

        public bool DeleteWorkflowAssociation(CSWorkflowAssociation association)
        {
            return base.CommitChanges
            (
                "WrkflwAssnDeleteByAssociationId",
                new Dictionary<string, object>()
                {
                    { "@WorkflowAssociationId", association.Id }
                }
            );
        }

        public void GetAssociationTriggers(CSWorkflowAssociation association, CSWorkflowTriggerEvents eventCollection)
        {
            DataSet ds = base.GetData
            (
                "WrkflwAssnGetEvents",
                new Dictionary<string, object>()
                {
                    { "@WorkflowAssociationId", association.Id }
                }
            );

            if (base.HasData(ds))
            {
                DataRow row = ds.Tables[0].Rows[0];

                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.farm_created, row["subscribe_event_farm_created"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.farm_modified, row["subscribe_event_farm_modified"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.farm_deleted, row["subscribe_event_farm_deleted"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.site_created, row["subscribe_event_site_created"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.site_modified, row["subscribe_event_site_modified"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.site_deleted, row["subscribe_event_site_deleted"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.directory_created, row["subscribe_event_directory_created"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.directory_modified, row["subscribe_event_directory_modified"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.directory_deleted, row["subscribe_event_directory_deleted"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.file_created, row["subscribe_event_file_created"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.file_modified, row["subscribe_event_file_modified"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.file_deleted, row["subscribe_event_file_deleted"]);
            }
        }

        public void GetWorkflowTriggers(CSWorkflowDefinition definition, CSWorkflowTriggerEvents eventCollection)
        {
            DataSet ds = base.GetData
            (
                "WrkflwDefnGetEvents",
                new Dictionary<string, object>()
                {
                    { "@WorkflowDefinitionId", definition.Id }
                }
            );

            if (base.HasData(ds))
            {
                DataRow row = ds.Tables[0].Rows[0];

                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.farm_created, row["allow_event_farm_created"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.farm_modified, row["allow_event_farm_modified"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.farm_deleted, row["allow_event_farm_deleted"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.site_created, row["allow_event_site_created"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.site_modified, row["allow_event_site_modified"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.site_deleted, row["allow_event_site_deleted"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.directory_created, row["allow_event_directory_created"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.directory_modified, row["allow_event_directory_modified"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.directory_deleted, row["allow_event_directory_deleted"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.file_created, row["allow_event_file_created"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.file_modified, row["allow_event_file_modified"]);
                addEventToCollection(eventCollection, WorkflowTriggerEventNamesEnum.file_deleted, row["allow_event_file_deleted"]);
            }
        }

        
        private void addEventToCollection(CSWorkflowTriggerEvents eventCollection, WorkflowTriggerEventNamesEnum eventToAdd, object eventFlag)
        {
            bool eventPresent = Utility.SafeConvertToBool(eventFlag);
            if (eventPresent)
            {
                eventCollection.Add(eventToAdd, true);
            }
        }

        public CSWorkflowDefinition GetWorkflowDefinition(Guid id)
        {
            DataSet ds = base.GetData
            (
                "WrkflwDefnGetById",
                new Dictionary<string, object>()
                {
                    { "@Id", id }
                }
            );

            if (! base.HasData(ds))
            {
                return null;
            }

            CSWorkflowDefinition definition = PopulateWorkflowDefinition(ds.Tables[0].Rows[0]);
            definition.Farm = _farm;
            return definition;
        }

        public CSWorkflowDefinition GetWorkflowDefinition(string name)
        {
            DataSet ds = base.GetData
            (
                "WrkflwDefnGetByName",
                new Dictionary<string, object>()
                {
                    { "@Name", name }
                }
            );

            if (!base.HasData(ds))
            {
                return null;
            }

            CSWorkflowDefinition definition = PopulateWorkflowDefinition(ds.Tables[0].Rows[0]);
            definition.Farm = _farm;
            return definition;
        }

        public List<CSWorkflowDefinition> GetAllWorkflowDefinitions()
        {
            List<CSWorkflowDefinition> list = new List<CSWorkflowDefinition>();

            DataSet ds = base.GetData
            (
                "WrkflwDefnGetAll"
            );

            if (base.HasData(ds))
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    CSWorkflowDefinition definition = PopulateWorkflowDefinition(row);
                    definition.Farm = _farm;
                    list.Add(definition);
                }
            }

            return list;
        }

        public CSWorkflowManifest GetWorkflowManifestForDefinition(CSWorkflowDefinition definition)
        {
            CSWorkflowManifest manifest = null;

            DataSet ds = base.GetData
            (
                "WrkflwManifestGetManifestForDefinition",
                new Dictionary<string, object>()
                {
                    { "@WorkflowDefinitionId", definition.Id }
                }
            );

            if (base.HasData(ds))
            {
                DataRow row = ds.Tables[0].Rows[0];

                manifest = new CSWorkflowManifest()
                {
                    Id = Utility.SafeConvertToGuid(row["Id"]),
                    WorkflowDefinition = definition,
                    WorkflowEngine = (WorkflowEngineEnum)Enum.Parse(typeof(WorkflowEngineEnum), Utility.SafeString(row["WorkflowEngine"])),
                    OutputAssemblyName = Utility.SafeString(row["OutputAssemblyName"]),
                    WorkflowClassName = Utility.SafeString(row["WorkflowClassName"]),

                    AlwaysCompile = Utility.SafeConvertToBool(row["always_compile"]),
                    CacheCompileResults = Utility.SafeConvertToBool(row["cache_compile_results"]),
                    LastCompiled = Utility.SafeConvertToDateTime(row["last_compiled_datetime"]),

                    BuildAssemblyTitle = Utility.SafeString(row["build_assembly_title"]),
                    BuildAssemblyDescription = Utility.SafeString(row["build_assembly_description"]),
                    BuildAssemblyCompany = Utility.SafeString(row["build_assembly_company"]),
                    BuildAssemblyProduct = Utility.SafeString(row["build_assembly_product"]),
                    BuildAssemblyCopyright = Utility.SafeString(row["build_assembly_copyright"]),
                    BuildAssemblyTrademark = Utility.SafeString(row["build_assembly_trademark"]),
                    BuildAssemblyVersion = new Version(Utility.SafeString(row["build_assembly_version"], "1.0.0.0")),
                    BuildAssemblyFileVersion = new Version(Utility.SafeString(row["build_assembly_fileversion"], "1.0.0.0")),

                    Created = Utility.SafeConvertToDateTime(row["Created"]),
                    CreatedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["CreatedBy"])),
                    Modified = Utility.SafeConvertToDateTime(row["Modified"]),
                    ModifiedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["ModifiedBy"]))
                };
            }

            return manifest;
        }

        public List<CSWorkflowManifestItem> GetManifestItemsForManifest(CSWorkflowManifest manifest)
        {
            List<CSWorkflowManifestItem> list = new List<CSWorkflowManifestItem>();

            DataSet ds = base.GetData
            (
                "WrkflwManifestGetManifestItemForManifest",
                new Dictionary<string, object>()
                {
                    { "@WorkflowManifestId", manifest.Id }
                }
            );

            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    CSWorkflowManifestItem item = new CSWorkflowManifestItem()
                    {
                        Id = Utility.SafeConvertToGuid(row["Id"]),
                        WorkflowManifest = manifest,
                        Filename = Utility.SafeString(row["Filename"]),
                        FilenameExtension = Utility.SafeString(row["FilenameExtension"]),
                        ItemType = (WorkflowManifestItemTypeEnum)Enum.Parse(typeof(WorkflowManifestItemTypeEnum), Utility.SafeString(row["ItemType"])),
                        RequiredForExecution = Utility.SafeConvertToBool(row["required_for_execution"]),
                        BuildtimeRelativeFolder = Utility.SafeString(row["build_relative_folder"]),
                        RuntimeRelativeFolder = Utility.SafeString(row["runtime_folder"]),
                        FileContent = (byte[])row["ContentStream"],

                        Created = Utility.SafeConvertToDateTime(row["Created"]),
                        CreatedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["CreatedBy"])),
                        Modified = Utility.SafeConvertToDateTime(row["Modified"]),
                        ModifiedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["ModifiedBy"]))
                    };

                    list.Add(item);
                }
            }

            return list;
        }

        public List<CSWorkflowAssociation> GetAllWorkflowAssociationsForDefinition(CSWorkflowDefinition definition)
        {
            List<CSWorkflowAssociation> list = new List<CSWorkflowAssociation>();

            DataSet ds = base.GetData
            (
                "WrkflwAssnGetByDefinitionId", 
                new Dictionary<string, object>()
                {
                    { "@WorkflowDefinitionId", definition.Id }
                }
            );

            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateWorkflowAssociation(row));
                }
            }

            return list;
        }

        public List<CSWorkflowAssociation> GetAllWorkflowAssociationsForFarm()
        {
            List<CSWorkflowAssociation> list = new List<CSWorkflowAssociation>();

            DataSet ds = base.GetData
            (
                "WrkflwAssnGetForFarm", 
                new Dictionary<string, object>()
                {
                    { "@FarmId", _farm.Id }
                }
            );

            if (base.HasData(ds))
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateWorkflowAssociation(row));
                }
            }

            return list;
        }

        public List<CSWorkflowAssociation> GetAllWorkflowAssociationsForSite(CSSite site)
        {
            List<CSWorkflowAssociation> list = new List<CSWorkflowAssociation>();

            DataSet ds = base.GetData
            (
                "WrkflwAssnGetForSite", 
                new Dictionary<string, object>()
                {
                    { "@SiteId", site.Id }
                }
            );

            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateWorkflowAssociation(row));
                }
            }

            return list;
        }

        public List<CSWorkflowAssociation> GetAllWorkflowAssociationsForDirectory(CSSite site, CSFileSystemEntryDirectory directory)
        {
            List<CSWorkflowAssociation> list = new List<CSWorkflowAssociation>();

            DataSet ds = base.GetData
            (
                "WrkflwAssnGetForDirectory",
                new Dictionary<string, object>()
                {
                    { "@SiteId", site.Id },
                    { "@DirectoryId", directory.Id }
                }
            );

            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateWorkflowAssociation(row));
                }
            }

            return list;
        }

        public CSWorkflowAssociation GetWorkflowAssociationById(Guid id)
        {
            DataSet ds = base.GetData
            (
                "WrkflwAssnGetById",
                new Dictionary<string, object>()
                {
                    { "@WorkflowAssociationId", id }
                }
            );

            if (! base.HasData(ds))
            {
                return null;
            }

            return PopulateWorkflowAssociation(ds.Tables[0].Rows[0]);
        }

        public List<CSWorkflowInstance> GetAllRunnableInstances()
        {
            List<CSWorkflowInstance> list = new List<CSWorkflowInstance>();

            DataSet ds = base.GetData
            (
                "WrkflwInstGetAllRunnable"
            );

            if (base.HasData(ds))
            {
                Dictionary<Guid, CSWorkflowAssociation> _assocCache = new Dictionary<Guid, CSWorkflowAssociation>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Guid assocId = Utility.SafeConvertToGuid(row["WorkflowAssociationId"]);

                    if (! assocId.Equals(Guid.Empty))
                    {
                        CSWorkflowAssociation rowAssociation = null;

                        if (!_assocCache.ContainsKey(assocId))
                        {
                            rowAssociation = GetWorkflowAssociationById(assocId);
                            if (rowAssociation != null)
                            {
                                _assocCache.Add(assocId, rowAssociation);
                            }
                        }
                        else
                        {
                            rowAssociation = _assocCache[assocId];
                        }

                        if (rowAssociation != null)
                        {
                            rowAssociation = _assocCache[assocId];
                            list.Add(PopulateWorkflowInstance(row, rowAssociation));
                        }
                    }
                }
            }

            return list;
        }

        public List<CSWorkflowInstance> GetInstancesForAssociation(CSWorkflowAssociation association, bool onlyRunnable = true)
        {
            List<CSWorkflowInstance> list = new List<CSWorkflowInstance>();

            DataSet ds = base.GetData
            (
                "WrkflwInstGetByAssociation",
                new Dictionary<string, object>()
                {
                    { "@WorkflowAssociationId", association.Id },
                    { "@only_runnable", onlyRunnable }
                }
            );

            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateWorkflowInstance(row, association));
                }
            }

            return list;
        }

        public List<CSWorkflowInstance> GetInstancesForDefinition(CSWorkflowDefinition definition, bool onlyRunnable = true)
        {
            List<CSWorkflowInstance> list = new List<CSWorkflowInstance>();

            DataSet ds = base.GetData
            (
                "WrkflwInstGetByDefinition",
                new Dictionary<string, object>()
                {
                    { "@WorkflowDefinitionId", definition.Id },
                    { "@only_runnable", onlyRunnable }
                }
            );

            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Guid associationId = Utility.SafeConvertToGuid(row["WorkflowAssociationId"]);
                    if (! associationId.Equals(Guid.Empty))
                    {
                        list.Add(PopulateWorkflowInstance(row, GetWorkflowAssociationById(associationId)));
                    }
                }
            }

            return list;
        }

        public CSWorkflowInstance GetInstanceById(Guid id, CSWorkflowAssociation association)
        {
            DataSet ds = base.GetData
            (
                "WrkflwInstGetById",
                new Dictionary<string, object>()
                {
                    { "@Id", id }
                }
            );

            if (! base.HasData(ds))
            {
                return null;
            }

            return PopulateWorkflowInstance(ds.Tables[0].Rows[0], association);
        }

        public CSWorkflowHistoryChain GetHistoryByInstance(CSWorkflowInstance instance)
        {
            CSWorkflowHistoryChain history = null;

            DataSet ds = base.GetData
            (
                "WrkflwHistGetByInstance",
                new Dictionary<string, object>()
                {
                    { "@WorkflowInstanceId", instance.Id }
                }
            );

            if (base.HasData(ds))
            {
                history = new CSWorkflowHistoryChain();

                foreach (DataRow row in ds.Tables[0].Select("", "[Created] DESC"))
                {
                    history.AddLast(PopulateWorkflowHistory(row, instance));
                }
            }

            return history;
        }

        public CSWorkflowHistoryChain GetHistoryByAssociation(CSWorkflowAssociation association)
        {
            CSWorkflowHistoryChain history = null;

            DataSet ds = base.GetData
            (
                "WrkflwHistGetByAssociation",
                new Dictionary<string, object>()
                {
                    { "@WorkflowAssociationId", association.Id }
                }
            );

            if (base.HasData(ds))
            {
                history = new CSWorkflowHistoryChain();

                foreach (DataRow row in ds.Tables[0].Select("", "[Created] DESC"))
                {
                    history.AddLast(PopulateWorkflowHistory(row, association));
                }
            }

            return history;
        }

        private CSWorkflowHistory PopulateWorkflowHistory(DataRow row, CSWorkflowAssociation association)
        {
            CSWorkflowHistory history = new CSWorkflowHistory()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                Instance = GetInstanceById(Utility.SafeConvertToGuid(row["WorkflowInstanceId"]), association),
                Association = association,

                State = (CSWorkflowEventTypesEnum)Utility.SafeConvertToInt(row["State"]),
                CompletedReason = (CSWorkflowEventCompletionTypesEnum)Utility.SafeConvertToInt(row["CompletedReason"]),
                ErrorMessage = Utility.SafeString(row["ErrorMessage"]),

                Created = Utility.SafeConvertToDateTime(row["Created"]),
                CreatedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["CreatedBy"]))
            };

            history.Site = history.Instance.Site;
            history.InstantiatedEntity = history.Instance.InstantiableEntity;

            return history;
        }

        private CSWorkflowHistory PopulateWorkflowHistory(DataRow row, CSWorkflowInstance instance)
        {
            return new CSWorkflowHistory()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                Instance = instance,
                Association = instance.Association,

                Site = instance.Site,
                InstantiatedEntity = instance.InstantiableEntity,

                State = (CSWorkflowEventTypesEnum)Utility.SafeConvertToInt(row["State"]),
                CompletedReason = (CSWorkflowEventCompletionTypesEnum)Utility.SafeConvertToInt(row["CompletedReason"]),
                ErrorMessage = Utility.SafeString(row["ErrorMessage"]),

                Created = Utility.SafeConvertToDateTime(row["Created"]),
                CreatedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["CreatedBy"]))
            };
        }

        private CSWorkflowInstance PopulateWorkflowInstance(DataRow row, CSWorkflowAssociation association)
        {
            if (association == null)
            {
                association = GetWorkflowAssociationById(Utility.SafeConvertToGuid(row["WorkflowAssociationId"]));
            }

            CSWorkflowInstance instance = new CSWorkflowInstance()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                InstanceInformation = Utility.SafeString(row["AssociationData"], null),
                Association = association,
                Site = null,
                InstantiableEntity = null,
                CurrentState = (CSWorkflowEventTypesEnum)Utility.SafeConvertToInt(row["CurrentState"]),
                CompletedReason = (CSWorkflowEventCompletionTypesEnum)Utility.SafeConvertToInt(row["CompletedReason"]),
                ErrorMessage = Utility.SafeString(row["ErrorMessage"]), 

                IsLoadedInRuntime = Utility.SafeConvertToBool(row["IsLoadedInRuntime"]),

                WorkflowPeristenceId = Utility.SafeConvertToGuid(row["SqlPersistenceId"])
            };

            Guid siteId = Utility.SafeConvertToGuid(row["SiteId"]);
            if (_site.Id.Equals(siteId))
            {
                instance.Site = _site;
            }
            else if (siteId.Equals(Guid.Empty))
            {
                instance.Site = _farm.DefaultSite;
            }
            else
            {
                instance.Site = _odmSite.GetById(siteId);
            }

            Guid entityId = Utility.SafeConvertToGuid(row["AssociatedContainerId"]);
            // look up container registry
            string type = GetTypeFromEntityRegistry(entityId);

            if (string.IsNullOrEmpty(type))
            {
                // this should NEVER happen
                throw new DataException("Entity Id is null. This should never happen. EntityRegistry is corrupted.");
            }

            switch (type.ToLower())
            {
                case "site":
                    if (entityId.Equals(_site.Id))
                    {
                        instance.InstantiableEntity = _site;
                    }
                    else if (entityId.Equals(instance.Site))
                    {
                        instance.InstantiableEntity = instance.Site;
                    }
                    else
                    {
                        instance.InstantiableEntity = _odmSite.GetById(entityId);
                    }
                    break;

                case "directory":
                case "file":
                    instance.InstantiableEntity = (IWorkflowInstantiable)(new OdmFileSystemDriver(instance.Site)).GetById(entityId);
                    break;
            }

            // if entity is still null, it is the farm
            if ((instance.InstantiableEntity == null) && (entityId == Guid.Empty))
            {
                instance.InstantiableEntity = CSFarm.Open(_site.AuthenticatedUser);
            }

            return instance;
        }

        private CSWorkflowDefinition PopulateWorkflowDefinition(DataRow row)
        {
            return new CSWorkflowDefinition()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                Name = Utility.SafeString(row["Name"]),
                Description = Utility.SafeString(row["Description"]),
                DefaultAssociationData = Utility.SafeString(row["DefaultAssociationData"]),

                AllowManualStart = Utility.SafeConvertToBool(row["allow_manual_start"]),
                StartOnCreate = Utility.SafeConvertToBool(row["start_on_create"]),
                StartOnModify = Utility.SafeConvertToBool(row["start_on_modify"]),
                IsEnabled = Utility.SafeConvertToBool(row["is_enabled"]),

                Created = Utility.SafeConvertToDateTime(row["Created"]),
                CreatedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["CreatedBy"])),
                Modified = Utility.SafeConvertToDateTime(row["Modified"]),
                ModifiedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["ModifiedBy"]))
            };
        }

        private CSWorkflowAssociation PopulateWorkflowAssociation(DataRow row)
        {
            CSWorkflowAssociation assoc = new CSWorkflowAssociation()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                WorkflowDefinition = GetWorkflowDefinition(Utility.SafeConvertToGuid(row["WorkflowDefinitionId"])),
                Name = Utility.SafeString(row["Name"]),
                CustomAssociationInformation = Utility.SafeString(row["AssociationData"]),
                Site = null,
                AssociatedEntity = null,
                AllowManualStart = Utility.SafeConvertToBool(row["allow_manual_start"]),
                StartOnCreate = Utility.SafeConvertToBool(row["start_on_create"]),
                StartOnModify = Utility.SafeConvertToBool(row["start_on_modify"]),
                IsEnabled = Utility.SafeConvertToBool(row["is_enabled"]),
                PreventStartingNewInstances = Utility.SafeConvertToBool(row["prevent_new_instances"]),

                Created = Utility.SafeConvertToDateTime(row["Created"]),
                CreatedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["CreatedBy"])),
                Modified = Utility.SafeConvertToDateTime(row["Modified"]),
                ModifiedBy = CSUser.GetById(Utility.SafeConvertToGuid(row["ModifiedBy"]))
            };

            Guid siteId = Utility.SafeConvertToGuid(row["SiteId"]);
            if (_site.Id.Equals(siteId))
            {
                assoc.Site = _site;
            }
            else if (siteId.Equals(Guid.Empty))
            {
                assoc.Site = _farm.DefaultSite;
            }
            else
            {
                assoc.Site = _odmSite.GetById(siteId);
            }

            Guid entityId = Utility.SafeConvertToGuid(row["AssociatedContainerId"]);
            // look up container registry
            string type = GetTypeFromEntityRegistry(entityId);

            if (string.IsNullOrEmpty(type))
            {
                // this should NEVER happen
                throw new DataException("Entity Id is null. This should never happen. EntityRegistry is corrupted.");
            }

            switch (type.ToLower())
            {
                case "site":
                    if (entityId.Equals(_site.Id))
                    {
                        assoc.AssociatedEntity = _site;
                    }
                    else if (entityId.Equals(assoc.Site))
                    {
                        assoc.AssociatedEntity = assoc.Site;
                    }
                    else
                    {
                        assoc.AssociatedEntity = _odmSite.GetById(entityId);
                    }
                    break;

                case "directory":
                case "file":
                    assoc.AssociatedEntity = (IWorkflowAssociable)(new OdmFileSystemDriver(assoc.Site)).GetById(entityId);
                    break;
            }

            // if entity is still null, it is the farm
            if ((assoc.AssociatedEntity == null) && (entityId == Guid.Empty))
            {
                assoc.AssociatedEntity = CSFarm.Open(_site.AuthenticatedUser);
            }

            return assoc;
        }

    }
}
