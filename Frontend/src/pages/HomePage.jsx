import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCar, faShieldAlt, faComments, faCheckCircle, faChartLine, faUsers } from '@fortawesome/free-solid-svg-icons';
import { Carousel } from 'react-responsive-carousel';
import 'react-responsive-carousel/lib/styles/carousel.min.css';

const HomePage = () => {
  return (
    <>
      <div style={{ width: '100vw', position: 'relative', left: '50%', right: '50%', marginLeft: '-50vw', marginRight: '-50vw' }}>
        
        <div className="text-white text-center py-5" style={{
          background: 'linear-gradient(135deg, #0a2b4e, #0d47a1, #1565c0)',
          boxShadow: '0 4px 20px rgba(0,0,0,0.2)'
        }}>
          <h1 className="display-2 fw-bold mb-3" style={{ textShadow: '2px 2px 4px rgba(0,0,0,0.3)' }}>
            AutoTrust
          </h1>
          <p className="lead fs-3 mb-4">Социальная сеть для купли-продажи автомобилей</p>
          <div className="d-flex justify-content-center gap-3 flex-wrap">
            <div className="hero-badge d-flex align-items-center gap-2 px-3 py-2 rounded-pill" style={{ backgroundColor: 'rgba(255,255,255,0.12)' }}>
                <FontAwesomeIcon icon={faCheckCircle} className="text-white" />
                <span className="text-white">Проверенные объявления</span>
             </div>
        <div className="hero-badge d-flex align-items-center gap-2 px-3 py-2 rounded-pill" style={{ backgroundColor: 'rgba(255,255,255,0.12)' }}>
            <FontAwesomeIcon icon={faChartLine} className="text-white" />
            <span className="text-white">Честная история</span>
        </div>
        <div className="hero-badge d-flex align-items-center gap-2 px-3 py-2 rounded-pill" style={{ backgroundColor: 'rgba(255,255,255,0.12)' }}>
            <FontAwesomeIcon icon={faUsers} className="text-white" />
            <span className="text-white">Безопасные сделки</span>
        </div>
        </div>
        </div>

        <Carousel showThumbs={false} autoPlay infiniteLoop interval={5000} showStatus={false}>
          <div style={{ height: '315px' }}>
            <img src="https://sun9-57.userapi.com/s/v1/ig2/eJ6yd5FMFQISceWYXf81DaVI4iT8IFcLc1jyUeuyM7VF916qGnzanoHt5fDtfnyOqMHF2WwSvodVwfaNk60KL3u9.jpg?quality=95&as=32x6,48x9,72x13,108x19,160x29,240x43,360x65,480x86,540x97,640x115,720x129,1080x194,1280x230,1440x259,1536x276&from=bu&u=h1mUX6cJ9_qEurjxC81iEY9Kr_DEDvhTpuIzBK1QMt0&cs=1536x0" style={{ height: '100%', width: '100%', objectFit: 'cover' }} />
          </div>
          <div style={{ height: '315px' }}>
            <img src="https://sun9-45.userapi.com/s/v1/ig2/EiykUB4yuokuASJW2IymiN6e8Ir5J8SEf8u-i4WSwcsXhFjzve8YzEoepDR6I4AE_s36T4pvy4Z5OBh10hU_i4kl.jpg?quality=95&as=32x5,48x7,72x11,108x17,160x25,240x37,360x56,480x74,540x84,640x99,720x112,1080x167,1280x198,1440x223,1529x237&from=bu&u=DIlRFAG8uxBmDj6O1ce6_X9o9zoebXncCyndHhHc7Ew&cs=1529x0" style={{ height: '100%', width: '100%', objectFit: 'cover' }} />
          </div>
          <div style={{ height: '315px' }}>
            <img src="https://sun9-42.userapi.com/s/v1/ig2/m0EFPHSXo2vQzbvbatfaz-W9D864_ObCruGfpWbrElYGlFvSjGhgMcOw7iis4q0EGu71siJyW3j4-RSjG07Ftcv1.jpg?quality=95&as=32x5,48x7,72x11,108x16,160x24,240x37,360x55,480x73,540x82,640x98,720x110,1080x165,1280x195,1440x220,1533x234&from=bu&u=-dcFd8rfL0LzI0cdL0lQayu7wONmBQZ4UNmMQqARJYY&cs=1533x0" style={{ height: '100%', width: '100%', objectFit: 'cover' }} />
          </div>
          <div style={{ height: '315px' }}>
            <img src="https://sun9-56.userapi.com/s/v1/ig2/3WFbEjkp5x6HPaX-H0JjngCh9NWccSe0ZlVXYqxWvE7PdEur-18bYZwsv9fyev4s_E5obwk3HUCYxbzM-St5BbQy.jpg?quality=95&as=32x5,48x8,72x12,108x18,160x26,240x40,360x59,480x79,540x89,640x105,720x119,1080x178,1280x211,1440x237,1536x253&from=bu&u=GI18y_JOnpH0heWqPr_9IP_u8nT2ran2fbu0aE4cUd0&cs=1536x0" style={{ height: '100%', width: '100%', objectFit: 'cover' }} />
          </div>
        </Carousel>
      </div>

      <div style={{ maxWidth: '1400px', margin: '0 auto', padding: '0 15px' }}>
        
        <div className="row text-center g-4 mt-5">
          <div className="col-md-4">
            <div className="card h-100 shadow-sm border-0">
              <div className="card-body">
                <FontAwesomeIcon icon={faCar} size="3x" className="mb-3 text-primary" />
                <h5 className="card-title">История владения</h5>
                <p className="card-text">Прозрачная цепочка владельцев, подтверждённая документально.</p>
              </div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="card h-100 shadow-sm border-0">
              <div className="card-body">
                <FontAwesomeIcon icon={faShieldAlt} size="3x" className="mb-3 text-primary" />
                <h5 className="card-title">Безопасные сделки</h5>
                <p className="card-text">Передача владения через площадку с подтверждением ДКП.</p>
              </div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="card h-100 shadow-sm border-0">
              <div className="card-body">
                <FontAwesomeIcon icon={faComments} size="3x" className="mb-3 text-primary" />
                <h5 className="card-title">Общение с продавцом</h5>
                <p className="card-text">Встроенный чат для переговоров без посредников.</p>
              </div>
            </div>
          </div>
        </div>

        <div className="row mt-5 pt-4">
          <div className="col-12 text-center">
            <h2>Как это работает</h2>
            <p className="text-muted">Три простых шага</p>
          </div>
          <div className="col-md-4 text-center mt-3">
            <div className="bg-primary rounded-circle d-flex align-items-center justify-content-center mx-auto" style={{ width: '70px', height: '70px', fontSize: '28px', color: 'white' }}>
              1
            </div>
            <h5 className="mt-3">Регистрация</h5>
            <p>Создайте аккаунт и заполните профиль</p>
          </div>
          <div className="col-md-4 text-center mt-3">
            <div className="bg-primary rounded-circle d-flex align-items-center justify-content-center mx-auto" style={{ width: '70px', height: '70px', fontSize: '28px', color: 'white' }}>
              2
            </div>
            <h5 className="mt-3">Разместите объявление</h5>
            <p>Добавьте машину или создайте запрос на покупку</p>
          </div>
          <div className="col-md-4 text-center mt-3">
            <div className="bg-primary rounded-circle d-flex align-items-center justify-content-center mx-auto" style={{ width: '70px', height: '70px', fontSize: '28px', color: 'white' }}>
              3
            </div>
            <h5 className="mt-3">Общайтесь и продавайте</h5>
            <p>Встроенный чат и безопасная передача владения</p>
          </div>
        </div>

        <footer className="mt-5 pt-3 text-center text-muted border-top">
          <p>&copy; 2025 AutoTrust. Все права защищены.</p>
        </footer>
      </div>
    </>
  );
};

export default HomePage;