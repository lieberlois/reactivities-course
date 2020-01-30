import React, { useContext, Fragment } from "react";
import { Item, Label } from "semantic-ui-react";
import { IActivity } from "../../../app/models/activity";
import { observer } from "mobx-react-lite";
import ActivityStore from "../../../app/stores/activityStore";
import { ActivityListItem } from "./ActivityListItem";

const ActivityList: React.FC = () => {
  const activityStore = useContext(ActivityStore);
  const { activitiesByDate } = activityStore;
  return (
    <>
      {activitiesByDate.map(([date, activities]) => ( // Create a group for each date
        <Fragment key={date}>
          <Label key={date} size="large" color="blue">
            {date}
          </Label>
            <Item.Group divided>
              {activities.map((activity: IActivity) => (  // Create an item for each element of the group
                <ActivityListItem key={activity.id} activity={activity} />
              ))}
            </Item.Group>
        </Fragment>
      ))}
    </>
  );
};

export default observer(ActivityList);
