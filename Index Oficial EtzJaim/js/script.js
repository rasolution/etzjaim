$(document).ready(function() {
   $('img').mouseenter(function() {
       $(this).animate({
           height: '+=10px'
       });
   });
   $('img').mouseleave(function() {
       $(this).animate({
           height: '-=10px'
       }); 
   });
   $('img').click(function() {
       $(this).toggle(1000);
   }); 
});