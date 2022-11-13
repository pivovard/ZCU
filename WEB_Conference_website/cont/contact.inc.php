                    <h1>Contact</h1>
                    
                    <div id="map" style="width:100%;height:400px;background:yellow"></div>
                    
                    <script>
                        function myMap() {
                            var myCenter = new google.maps.LatLng(49.7265837, 13.3524429);
                            var mapCanvas = document.getElementById("map");
                            var mapOptions = {center: myCenter, zoom: 16};
                            var map = new google.maps.Map(mapCanvas, mapOptions);
                            var marker = new google.maps.Marker({position: myCenter,animation: google.maps.Animation.BOUNCE});
                            marker.setMap(map);
                        }
                    </script>
                    
                    <script src="https://maps.googleapis.com/maps/api/js?callback=myMap"></script>