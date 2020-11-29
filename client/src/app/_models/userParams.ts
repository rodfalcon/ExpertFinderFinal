import { User } from './user';

export class UserParams {
    area: string;
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'lastActive';

    constructor(user: User) {
      // this.area = user.area === 'Fitness & Health' ? 'Fitness & Health' : 'IT' ? 'IT' : 'Spa and Beauty' ? 'Spa and Beauty' : 'Home and Repair'
      //   ? 'Home and Repair' : 'Courier' ? 'Courier' : '';

      this.area = user.area;
      if (this.area == 'Fitness & Health')
      {
        this.area = 'Fitness & Health';
      } else if (this.area == 'IT') {
        this.area = 'IT';
      } else if (this.area == 'Spa and Beauty') {
        this.area = 'Spa and Beauty';
      } else if (this.area == 'Home and Repair') {
        this.area = 'Home and Repair';
      } else {
        this.area = 'Courier';
      }
    }
} 