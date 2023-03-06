# AR_ICP_Matching

해당 프로젝트는 Unity로 제작되었으나, 용량의 문제로 해당 프로젝트를 진행하면서 사용했던 스크립트 위주로 올렸습니다.

## 논문 제목 : AR에서 객체의 증강 위치를 효율적으로 보간하기 위한 새로운 ICP 매칭

AR에서 객체의 증강 위치를 효율적으로 보간하기 위한 새로운 ICP 매칭 알고리즘 제안
<br>진행기간 2021/12/29 - 2022/06/22
<img src="https://img.shields.io/badge/Unity-FFFFFF?style=for-the-badge&logo=Unity&logoColor=black">
<img src="https://img.shields.io/badge/Firebase-FFCA28?style=for-the-badge&logo=Firebase&logoColor=black">
<img src="https://img.shields.io/badge/Python-3776AB?style=for-the-badge&logo=Python&logoColor=white">

<aside>
📢 2022년 제 66차  한국컴퓨터 정보학회에 제출한 연구이며, 주 저자 입니다. 
Unity AR 상에서 Point Cloud를 효율적으로 매칭하여 증강하기 위한 연구를 진행하였습니다.

</aside>

- **문제 :**
    - 마커검출 방식은 사전에 마커를 지정할 수 없는 경우 사용이 어려움 반면에 마커리스는 불안정하여 대부분 평면 검출이나 페이스 검출을 사용
- **솔루션 :**
    - 본 논문에서는 평면 생성 없이 특징 포인트만을 이용하여 증강하기 위해 현실과의 점 구름을 비교 및 사용자의 위치를 기반으로 위치를 계산 후 증강
- **기대효과 :**
    - 모바일뿐만 아니라 도로 공사, 뇌 수술 등 높은 정확도가 필요한 상황에서 사용하여 더욱 안정적으로 사용 가능

![image](https://user-images.githubusercontent.com/76572665/223185421-88f6142d-c3ca-473e-9237-455f845bf356.png)


## 🖱 담당한 기능

- 평면 생성 없이 특징 포인트만을 이용하여 증강하여 현실과의 점 구름을 비교해 본 결과 실제와 매우 비슷한 점 구름 수집이 가능하며, 이전 알고리즘을 통해 비교하는 계산량을 줄임
- 클릭 위치를 바탕으로 증강 위치에 가중치를 두어 원하는 곳에 증강 및 점구름 수집 계산 효율을 높임
- 모바일에서 수집한 GPS 신호와 주변 환경의 특징점으로 구성된 점 구름을 이용하여 ICP매칭 기법을 통한 효율적인 증강현실 프레임워크를 개발하여 다양한 환경에서도 대부분 안정적인 결과가 나옴
- 실제 환경과 점구름을 시각적으로 비교하기 위해 Python을 이용하여 비교

## 💡 깨달은 점

- 많은 특징점들을 저장하기 위해 **Firebase를 사용하여 데이터를 다룸**
- 유니티에서 제공하지 않는 특정 라이브러리(PCL)사용을 위해 따로 **DLL로 만들어 내어 쉽게 사용 가능**
- Android에서는 C++코드가 돌아가지 않기 때문에, **NDK를 이용하여 작동**
- 다양한 AR 도구가 있었지만, **코드에 접근하기 쉬운 것이 AR Foundation**이라 이것을 사용하였다.
- 제공된 변수를 이용하기 위해 직접 코드 해석 (**코드해석능력** 증가)

## 기타

- [https://www.dbpia.co.kr/journal/articleDetail?nodeId=NODE11140491](https://www.dbpia.co.kr/journal/articleDetail?nodeId=NODE11140491)
- 해당 발표 영상은 [여기](https://www.youtube.com/watch?v=abS5pY5J6Hg)를 클릭하면 보실 수 있습니다.
