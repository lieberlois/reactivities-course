import React, { useContext, Fragment } from "react";
import { Item, Label } from "semantic-ui-react";
import { IActivity } from "../../../app/models/activity";
import { observer } from "mobx-react-lite";
import { ActivityListItem } from "./ActivityListItem";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { format } from "date-fns";

const ActivityList: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { activitiesByDate } = rootStore.activityStore;
  return (
    <>
      {activitiesByDate.map((
        [date, activities] // Create a group for each date
      ) => (
        <Fragment key={date}>
          <Label key={date} size="large" color="blue">
            {format(date, "eeee, do MMMM")}
          </Label>
          <Item.Group divided>
            {activities.map((
              activity: IActivity // Create an item for each element of the group
            ) => (
              <ActivityListItem key={activity.id} activity={activity} />
            ))}
          </Item.Group>
        </Fragment>
      ))}
    </>
  );
};

export default observer(ActivityList);
