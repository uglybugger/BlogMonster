@using BlogMonster.Domain.Entities
@using BlogMonster.Extensions
@model BlogMonster.Web.ViewModels.ArchiveViewModel
<div id="archiveWidget" class="widget">
    <h1>Archive</h1>
    @foreach (var year in Model.Posts)
    {
        <h2>@year.Key</h2>

        <div class="year">
            @foreach (var month in year.Value)
            {
                <h3>@month.Key</h3>
                <div class="month">
                    @foreach (BlogPost post in month.Value)
                    {
                        string tooltip = "{0}/{1}".FormatWith(post.PostDate.Day, post.PostDate.Month);
                        string linkText = post.Title.CoalesceIfWhiteSpace("Untitled Post");
                        @Html.ActionLink(linkText, "Post", "Blog", new {id = post.Permalinks.First()}, new {@class = "post", title = tooltip})<br />
                    }
                </div>
            }
        </div>
    }
</div>

<script type="text/javascript">

    ($(function() {
        $(document).ready(function() {
            $("#archiveWidget").delegate("#archiveWidget h2", "click", function() {
                var $div = $(this);
                $div.next("div.year").toggle();
            });

            $("#archiveWidget").delegate("#archiveWidget h3", "click", function() {
                var $div = $(this);
                $div.next("div.month").toggle();
            });

            var yearSelector = "#archiveWidget h2:contains('" + window.PostYear + "')";
            var monthSelector = "h3:contains('" + window.PostMonth + "')";
            var $year = $(yearSelector).next("div.year");
            var $month = $(monthSelector, $year).next("div.month");
            $year.show();
            $month.show();
        });
    }));
</script>