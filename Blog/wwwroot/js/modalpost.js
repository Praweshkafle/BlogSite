tinymce.init({
    selector: '#Content'
});

const fileInput = document.getElementById('file');
const openFileButton = document.getElementById('openFileButton');
const selectedImage = document.getElementById('selectedImage');
const changeButton = document.getElementById('change');
const deleteButton = document.getElementById('delete');
var globalPostId = 0;
const reader = new FileReader();
openFileButton.addEventListener('click', () => {
    fileInput.click();
});

fileInput.addEventListener('change', (e) => {
    const file = e.target.files[0];

    if (file) {
        reader.onload = (event) => {
            selectedImage.src = event.target.result;
            selectedImage.style.display = 'block';
            openFileButton.style.display = 'none';
            changeButton.style.display = 'inline-block';
            deleteButton.style.display = 'inline-block';
        };

        reader.readAsDataURL(file);
    } else {
        selectedImage.src = '';
    }
});
changeButton.addEventListener('click', () => {
    fileInput.click();
});

deleteButton.addEventListener('click', () => {
    selectedImage.src = '';
    fileInput.value = '';
    selectedImage.style.display = 'none';
    openFileButton.style.display = 'block';
    changeButton.style.display = 'none';
    deleteButton.style.display = 'none';
});

function submitBlogPost(id) {
    var data = {
        Id: globalPostId,
        Title: $('#Title').val(),
        Content: tinymce.get('Content').getContent(),
        Image: '',
        PublicationDate: "2023-11-07 14:35:25.697",
        AuthorId: 0
    };

    var formData = new FormData();

    formData.append("blogPostDto", JSON.stringify(data));
    formData.append("file", $("#file")[0].files[0] ? $("#file")[0].files[0] : null);

    $.ajax({
        url: "/blog/create",
        type: "POST",
        processData: false,  // Don't process the data
        contentType: false,  // Don't set content type
        data: formData,
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                $('#Title').val("");
                tinymce.get("Content").setContent("");
                init();
                $('#examplemodal').modal('hide');
                alert(response.message);
            } else {
                alert("Error: " + response.message);
            }
        },
        error: function () {
            alert("Error: An error occurred while making the request.");
        }
    });
}




$(document).ready(function () {
    $("#submitBtn").click(function (e) {
        e.preventDefault();
        var post = $(this).closest('.post');
        var postId = post.find('.post-id').val();
        submitBlogPost(postId);
    });
});


$(document).ready(function () {
    $('.edit-button').click(function (e) {
        e.preventDefault();
        $('#Title').val("");
        tinymce.get("Content").setContent("");
        globalPostId = 0;
        var modalContainer = $(this).closest('#examplemodal');
        var post = $(this).closest('.post');
        var postId = post.find('.post-id').val();
        globalPostId = postId;
        var url = "/blog/edit/" + postId;
        $.ajax({
            url: url,
            type: "GET",
            success: function (response) {
                init();
                if (response.image !== null && response.image !== "") {
                    selectedImage.src = "/Images/Custom/" + response.image;
                    selectedImage.style.display = 'block';
                    openFileButton.style.display = 'none';
                    changeButton.style.display = 'inline-block';
                    deleteButton.style.display = 'inline-block';
                }
                $('#Title').val(response.title);
                tinymce.get("Content").setContent(response.content);
                $('#examplemodal').modal('show');
            },
            error: function () {
                alert("Error: An error occurred while making the request.");
            }
        });
    });
});

function init() {
    selectedImage.src = "";
    selectedImage.style.display = 'none';
    openFileButton.style.display = 'block';
    changeButton.style.display = 'none';
    deleteButton.style.display = 'none';
}