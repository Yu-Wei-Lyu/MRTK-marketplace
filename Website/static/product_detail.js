function myFunction() {
    var text = "確定要登出?";
    if (confirm(text) == true) {
      window.location.href = "/";
    }
}