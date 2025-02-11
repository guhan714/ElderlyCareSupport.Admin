import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: 150, 
    duration: '150s', 
};

export default function () {

    const token = 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJBZG1pbiIsImlzcyI6IkVsZGVybHlDYXJlU3VwcG9ydCIsImV4cCI6MTczNzkwNzg3Nywic3ViIjoiYWRtaW5AZWxkZXJseWNhcmVzdXBwb3J0LmNvbSIsImVtYWlsIjoiYWRtaW5AZWxkZXJseWNhcmVzdXBwb3J0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiaWF0IjoxNzM3OTA0Mjc3LCJuYmYiOjE3Mzc5MDQyNzd9.ZYr_Nub4xnpuzCBboQaMH1eh6TuNaKJ6AjCpSbm_7_QPLnV-sbY8_fa3irRUZp3Df4DGbw6GQb1Meln0uOlOIg';

    const headers = {
        Authorization: `Bearer ${token}`,
        'MAC-Address': '70-A6-CC-23-D4-FD',
        'Content-Type': 'application/json',
    };

    const res = http.get('https://localhost:44374/api/UserManagement/users/user@example.com', { headers });

    check(res, {
        'status is 200': (r) => r.status === 200,
        'response time < 200ms': (r) => r.timings.duration < 200,
    });
}
