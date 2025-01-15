function tagHelper(blogContent, matchRegex, replacedRegex, tag) {
    const arrText = blogContent.innerText.match(matchRegex)
    if (arrText && arrText.length > 0) {
        arrText.forEach((val) => {
            var replacedVal = val.replace(replacedRegex, "")
            if (!(tag == "img" || tag == "iframe" || tag == "uli")) {
                blogContent.innerText = blogContent.innerText.replace(val, `<${tag}>${replacedVal}</${tag}>`)
            }
                
            switch (tag) {
                case "a":
                    var linkGenerate = replacedVal.split(" ");
                    if (linkGenerate.length > 1) {
                        val = replacedVal
                        blogContent.innerText = blogContent.innerText.replace(val, `<a href="${linkGenerate[0]}" target="_blank">${linkGenerate.slice(1, linkGenerate.length).join(" ")}</a>`)
                    } else if (linkGenerate.length == 1) {

                        blogContent.innerText = blogContent.innerText.replace(val, `<a href="${linkGenerate[0]}" target="_blank">${linkGenerate[0]}</a>`)
                    }
                    break;
                case "img":
                    if (replacedVal.startsWith("https://") || replacedVal.startsWith("http://"))
                        blogContent.innerText = blogContent.innerText.replace(val, `
                                <${tag} src="${replacedVal}" class="card-img-top" />
                                `)
                    break;
                case "iframe":
                    var linkGenerate = replacedVal.split(" ");
                    if (linkGenerate.length > 1)
                        blogContent.innerText = blogContent.innerText.replace(val, `
                                <${tag} class="card-img-top" height="350" src="${linkGenerate[0]}" title="${linkGenerate.slice(1, linkGenerate.length).join(" ")}" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></${tag}>`)
                    else if(linkGenerate.length == 1)
                        blogContent.innerText = blogContent.innerText.replace(val, `
                                <${tag} class="card-img-top" height="350" src="${linkGenerate[0]}" title="Video" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></${tag}>`)
                                break;
                case "uli":
                    blogContent.innerText = blogContent.innerText.replace(val, `<li class="ms-4">${replacedVal}</li>`)
                    break;
                default:
                    break;
            }
        })
    }
}

export default function TagSanitizer(contentID) {
    const blogContent = document.getElementById(contentID);
    blogContent.innerText = blogContent.innerText.replace(/>/gm, "&gt;");
    blogContent.innerText = blogContent.innerText.replace(/</gm, "&lt;");
    tagHelper(blogContent, /^% [^\n ]+( {1,}[a-zA-Z0-9_!@#$%*^&()\-;\\//=\[\]."]+)*\n?/gm, /% /gm, "uli"); //Unordered List
    tagHelper(blogContent, /\*[^\n\* ]+( {1,}[a-zA-Z0-9_!@#$%^&(?)\-;\\//=\[\]."]+)*\*/gm, /\*/gm, "b"); //Bold
    tagHelper(blogContent, /\`[^\n` ]+( {1,}[a-zA-Z0-9_!@#$%*^&(?)\-;\\//=\[\]."]+)*`/gm, /`/gm, "i"); //Italics
    tagHelper(blogContent, /\^[^\n\^ ]+( {1,}[a-zA-Z0-9_!@#$%*^&(?)\-;\\//=\[\]."]+)*\^/gm, /\^/gm, "h4"); //h4
    tagHelper(blogContent, /#[^\n# ]+( {1,}[a-zA-Z0-9_!@#*$'"%^&(?)\-;\\//=\[\]."]+)*#/gm, /#/gim, "h2"); //h2
    tagHelper(blogContent, /\[[^\n\[\] ]+([a-zA-Z0-9_!@#*$%^&();?\-\\//=\[\]."]+)*\]/gm, /[\[\]]/gm, "img"); //image
    tagHelper(blogContent, /@[^\n ]+( {1,}[a-zA-Z0-9_!@#*$%^&();?\-\\//=\[\]."]+)*@/gm, /@/gim, "a"); //link
    tagHelper(blogContent, /\+[^\n@ ]+( {1,}[a-zA-Z0-9_!@#*$%^&(?);\-\\//=\[\]."]+)*\+/gm, /\+/gim, "iframe"); //iframe
    blogContent.innerHTML = blogContent.innerText;
}