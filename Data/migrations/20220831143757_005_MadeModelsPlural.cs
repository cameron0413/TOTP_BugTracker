﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TOTP_BugTracker.data.migrations
{
    public partial class _005_MadeModelsPlural : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Company_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BTUserProject_Project_ProjectsId",
                table: "BTUserProject");

            migrationBuilder.DropForeignKey(
                name: "FK_Invite_AspNetUsers_InviteeId",
                table: "Invite");

            migrationBuilder.DropForeignKey(
                name: "FK_Invite_AspNetUsers_InvitorId",
                table: "Invite");

            migrationBuilder.DropForeignKey(
                name: "FK_Invite_Company_CompanyId",
                table: "Invite");

            migrationBuilder.DropForeignKey(
                name: "FK_Invite_Project_ProjectId",
                table: "Invite");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_RecipientId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_SenderId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationType_NotificationTypeId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Project_ProjectId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Ticket_TicketId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Company_CompanyId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_ProjectPriority_ProjectPriorityId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_DeveloperUserId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_SubmitterUserId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Project_ProjectId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketPriority_TicketPriorityId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketStatus_TicketStatusId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketType_TicketTypeId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachment_AspNetUsers_UserId",
                table: "TicketAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachment_Ticket_TicketId",
                table: "TicketAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketComment_AspNetUsers_UserId",
                table: "TicketComment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketComment_Ticket_TicketId",
                table: "TicketComment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistory_AspNetUsers_UserId",
                table: "TicketHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistory_Ticket_TicketId",
                table: "TicketHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketType",
                table: "TicketType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketStatus",
                table: "TicketStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketPriority",
                table: "TicketPriority");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketHistory",
                table: "TicketHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketComment",
                table: "TicketComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketAttachment",
                table: "TicketAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPriority",
                table: "ProjectPriority");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invite",
                table: "Invite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Company",
                table: "Company");

            migrationBuilder.RenameTable(
                name: "TicketType",
                newName: "TicketTypes");

            migrationBuilder.RenameTable(
                name: "TicketStatus",
                newName: "TicketStatuses");

            migrationBuilder.RenameTable(
                name: "TicketPriority",
                newName: "TicketPriorities");

            migrationBuilder.RenameTable(
                name: "TicketHistory",
                newName: "TicketHistories");

            migrationBuilder.RenameTable(
                name: "TicketComment",
                newName: "TicketComments");

            migrationBuilder.RenameTable(
                name: "TicketAttachment",
                newName: "TicketAttachments");

            migrationBuilder.RenameTable(
                name: "Ticket",
                newName: "Tickets");

            migrationBuilder.RenameTable(
                name: "ProjectPriority",
                newName: "ProjectPriorities");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "NotificationType",
                newName: "NotificationTypes");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Invite",
                newName: "Invites");

            migrationBuilder.RenameTable(
                name: "Company",
                newName: "Companies");

            migrationBuilder.RenameIndex(
                name: "IX_TicketHistory_UserId",
                table: "TicketHistories",
                newName: "IX_TicketHistories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketHistory_TicketId",
                table: "TicketHistories",
                newName: "IX_TicketHistories_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketComment_UserId",
                table: "TicketComments",
                newName: "IX_TicketComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketComment_TicketId",
                table: "TicketComments",
                newName: "IX_TicketComments_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketAttachment_UserId",
                table: "TicketAttachments",
                newName: "IX_TicketAttachments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketAttachment_TicketId",
                table: "TicketAttachments",
                newName: "IX_TicketAttachments_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_TicketTypeId",
                table: "Tickets",
                newName: "IX_Tickets_TicketTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_TicketStatusId",
                table: "Tickets",
                newName: "IX_Tickets_TicketStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_TicketPriorityId",
                table: "Tickets",
                newName: "IX_Tickets_TicketPriorityId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_SubmitterUserId",
                table: "Tickets",
                newName: "IX_Tickets_SubmitterUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_ProjectId",
                table: "Tickets",
                newName: "IX_Tickets_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_DeveloperUserId",
                table: "Tickets",
                newName: "IX_Tickets_DeveloperUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_ProjectPriorityId",
                table: "Projects",
                newName: "IX_Projects_ProjectPriorityId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_CompanyId",
                table: "Projects",
                newName: "IX_Projects_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_TicketId",
                table: "Notifications",
                newName: "IX_Notifications_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_SenderId",
                table: "Notifications",
                newName: "IX_Notifications_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_RecipientId",
                table: "Notifications",
                newName: "IX_Notifications_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_ProjectId",
                table: "Notifications",
                newName: "IX_Notifications_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_NotificationTypeId",
                table: "Notifications",
                newName: "IX_Notifications_NotificationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Invite_ProjectId",
                table: "Invites",
                newName: "IX_Invites_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Invite_InvitorId",
                table: "Invites",
                newName: "IX_Invites_InvitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Invite_InviteeId",
                table: "Invites",
                newName: "IX_Invites_InviteeId");

            migrationBuilder.RenameIndex(
                name: "IX_Invite_CompanyId",
                table: "Invites",
                newName: "IX_Invites_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketTypes",
                table: "TicketTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketStatuses",
                table: "TicketStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketPriorities",
                table: "TicketPriorities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketHistories",
                table: "TicketHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketComments",
                table: "TicketComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketAttachments",
                table: "TicketAttachments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPriorities",
                table: "ProjectPriorities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invites",
                table: "Invites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BTUserProject_Projects_ProjectsId",
                table: "BTUserProject",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_AspNetUsers_InviteeId",
                table: "Invites",
                column: "InviteeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_AspNetUsers_InvitorId",
                table: "Invites",
                column: "InvitorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Companies_CompanyId",
                table: "Invites",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Projects_ProjectId",
                table: "Invites",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_RecipientId",
                table: "Notifications",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_SenderId",
                table: "Notifications",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Projects_ProjectId",
                table: "Notifications",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Tickets_TicketId",
                table: "Notifications",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_CompanyId",
                table: "Projects",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectPriorities_ProjectPriorityId",
                table: "Projects",
                column: "ProjectPriorityId",
                principalTable: "ProjectPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachments_AspNetUsers_UserId",
                table: "TicketAttachments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachments_Tickets_TicketId",
                table: "TicketAttachments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_AspNetUsers_UserId",
                table: "TicketComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_Tickets_TicketId",
                table: "TicketComments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistories_AspNetUsers_UserId",
                table: "TicketHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistories_Tickets_TicketId",
                table: "TicketHistories",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_DeveloperUserId",
                table: "Tickets",
                column: "DeveloperUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterUserId",
                table: "Tickets",
                column: "SubmitterUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Projects_ProjectId",
                table: "Tickets",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketPriorities_TicketPriorityId",
                table: "Tickets",
                column: "TicketPriorityId",
                principalTable: "TicketPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusId",
                table: "Tickets",
                column: "TicketStatusId",
                principalTable: "TicketStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketTypes_TicketTypeId",
                table: "Tickets",
                column: "TicketTypeId",
                principalTable: "TicketTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BTUserProject_Projects_ProjectsId",
                table: "BTUserProject");

            migrationBuilder.DropForeignKey(
                name: "FK_Invites_AspNetUsers_InviteeId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Invites_AspNetUsers_InvitorId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Companies_CompanyId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Projects_ProjectId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_RecipientId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_SenderId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Projects_ProjectId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Tickets_TicketId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_CompanyId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectPriorities_ProjectPriorityId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachments_AspNetUsers_UserId",
                table: "TicketAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachments_Tickets_TicketId",
                table: "TicketAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_AspNetUsers_UserId",
                table: "TicketComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_Tickets_TicketId",
                table: "TicketComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistories_AspNetUsers_UserId",
                table: "TicketHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistories_Tickets_TicketId",
                table: "TicketHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_DeveloperUserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterUserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Projects_ProjectId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketPriorities_TicketPriorityId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketTypes_TicketTypeId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketTypes",
                table: "TicketTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketStatuses",
                table: "TicketStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketPriorities",
                table: "TicketPriorities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketHistories",
                table: "TicketHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketComments",
                table: "TicketComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketAttachments",
                table: "TicketAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPriorities",
                table: "ProjectPriorities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invites",
                table: "Invites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "TicketTypes",
                newName: "TicketType");

            migrationBuilder.RenameTable(
                name: "TicketStatuses",
                newName: "TicketStatus");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "Ticket");

            migrationBuilder.RenameTable(
                name: "TicketPriorities",
                newName: "TicketPriority");

            migrationBuilder.RenameTable(
                name: "TicketHistories",
                newName: "TicketHistory");

            migrationBuilder.RenameTable(
                name: "TicketComments",
                newName: "TicketComment");

            migrationBuilder.RenameTable(
                name: "TicketAttachments",
                newName: "TicketAttachment");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameTable(
                name: "ProjectPriorities",
                newName: "ProjectPriority");

            migrationBuilder.RenameTable(
                name: "NotificationTypes",
                newName: "NotificationType");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "Invites",
                newName: "Invite");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Company");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_TicketTypeId",
                table: "Ticket",
                newName: "IX_Ticket_TicketTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_TicketStatusId",
                table: "Ticket",
                newName: "IX_Ticket_TicketStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_TicketPriorityId",
                table: "Ticket",
                newName: "IX_Ticket_TicketPriorityId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_SubmitterUserId",
                table: "Ticket",
                newName: "IX_Ticket_SubmitterUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_ProjectId",
                table: "Ticket",
                newName: "IX_Ticket_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_DeveloperUserId",
                table: "Ticket",
                newName: "IX_Ticket_DeveloperUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketHistories_UserId",
                table: "TicketHistory",
                newName: "IX_TicketHistory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketHistories_TicketId",
                table: "TicketHistory",
                newName: "IX_TicketHistory_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketComments_UserId",
                table: "TicketComment",
                newName: "IX_TicketComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketComments_TicketId",
                table: "TicketComment",
                newName: "IX_TicketComment_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketAttachments_UserId",
                table: "TicketAttachment",
                newName: "IX_TicketAttachment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketAttachments_TicketId",
                table: "TicketAttachment",
                newName: "IX_TicketAttachment_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ProjectPriorityId",
                table: "Project",
                newName: "IX_Project_ProjectPriorityId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_CompanyId",
                table: "Project",
                newName: "IX_Project_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_TicketId",
                table: "Notification",
                newName: "IX_Notification_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_SenderId",
                table: "Notification",
                newName: "IX_Notification_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_RecipientId",
                table: "Notification",
                newName: "IX_Notification_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ProjectId",
                table: "Notification",
                newName: "IX_Notification_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notification",
                newName: "IX_Notification_NotificationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_ProjectId",
                table: "Invite",
                newName: "IX_Invite_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_InvitorId",
                table: "Invite",
                newName: "IX_Invite_InvitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_InviteeId",
                table: "Invite",
                newName: "IX_Invite_InviteeId");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_CompanyId",
                table: "Invite",
                newName: "IX_Invite_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketType",
                table: "TicketType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketStatus",
                table: "TicketStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketPriority",
                table: "TicketPriority",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketHistory",
                table: "TicketHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketComment",
                table: "TicketComment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketAttachment",
                table: "TicketAttachment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPriority",
                table: "ProjectPriority",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invite",
                table: "Invite",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Company",
                table: "Company",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Company_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BTUserProject_Project_ProjectsId",
                table: "BTUserProject",
                column: "ProjectsId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_AspNetUsers_InviteeId",
                table: "Invite",
                column: "InviteeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_AspNetUsers_InvitorId",
                table: "Invite",
                column: "InvitorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_Company_CompanyId",
                table: "Invite",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_Project_ProjectId",
                table: "Invite",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_RecipientId",
                table: "Notification",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_SenderId",
                table: "Notification",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationType_NotificationTypeId",
                table: "Notification",
                column: "NotificationTypeId",
                principalTable: "NotificationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Project_ProjectId",
                table: "Notification",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Ticket_TicketId",
                table: "Notification",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Company_CompanyId",
                table: "Project",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_ProjectPriority_ProjectPriorityId",
                table: "Project",
                column: "ProjectPriorityId",
                principalTable: "ProjectPriority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_DeveloperUserId",
                table: "Ticket",
                column: "DeveloperUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_SubmitterUserId",
                table: "Ticket",
                column: "SubmitterUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Project_ProjectId",
                table: "Ticket",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketPriority_TicketPriorityId",
                table: "Ticket",
                column: "TicketPriorityId",
                principalTable: "TicketPriority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketStatus_TicketStatusId",
                table: "Ticket",
                column: "TicketStatusId",
                principalTable: "TicketStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketType_TicketTypeId",
                table: "Ticket",
                column: "TicketTypeId",
                principalTable: "TicketType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachment_AspNetUsers_UserId",
                table: "TicketAttachment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachment_Ticket_TicketId",
                table: "TicketAttachment",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComment_AspNetUsers_UserId",
                table: "TicketComment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComment_Ticket_TicketId",
                table: "TicketComment",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistory_AspNetUsers_UserId",
                table: "TicketHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistory_Ticket_TicketId",
                table: "TicketHistory",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
