﻿using HokkaidoBookshop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HokkaidoBookshop.Utils.TagHelpers {
	public class PageLinkTagHelper : TagHelper {
		private readonly IUrlHelperFactory urlHelperFactory;
		public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory) {
			this.urlHelperFactory = urlHelperFactory;
		}

		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext ViewContext { get; set; } = null!;
		public PageViewModel? PageModel { get; set; }
		public string PageAction { get; set; } = "";

		[HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
		public Dictionary<string, object> PageUrlValues { get; set; } = new();

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			if (PageModel == null) throw new Exception("PageModel is not set");
			IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
			output.TagName = "div";

			TagBuilder tag = new("ul");
			tag.AddCssClass("pagination");

			TagBuilder currentItem = CreateTag(PageModel.PageNumber, urlHelper);

			if (PageModel.HasPreviousPage) {
				TagBuilder prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper);
				tag.InnerHtml.AppendHtml(prevItem);
			}

			tag.InnerHtml.AppendHtml(currentItem);
			if (PageModel.HasNextPage) {
				TagBuilder nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper);
				tag.InnerHtml.AppendHtml(nextItem);
			}
			output.Content.AppendHtml(tag);
		}

		TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper) {
			TagBuilder item = new("li");
			TagBuilder link = new("a");
			if (pageNumber == PageModel?.PageNumber) {
				item.AddCssClass("active");
			}
			else {
				PageUrlValues["page"] = pageNumber;
				link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
			}
			item.AddCssClass("page-item");
			link.AddCssClass("page-link");
			link.InnerHtml.Append(pageNumber.ToString());
			item.InnerHtml.AppendHtml(link);
			return item;
		}
	}
}
